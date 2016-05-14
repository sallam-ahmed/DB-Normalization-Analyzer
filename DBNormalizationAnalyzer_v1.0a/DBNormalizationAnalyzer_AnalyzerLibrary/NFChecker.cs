using System;
using System.Collections;
using System.Linq;

namespace DBNormalizationAnalyzer.AnalyzerLibrary
{
    public class NfChecker
    {
        public FunctionalDependency Fd { get; set; }
        public NfChecker()
        {
            Fd = null;
        }

        public NfChecker(FunctionalDependency fd)
        {
            Fd = fd;
        }

        public Error Check()
        {
            var res = FirstNormalForm();
            if (res.Level == 1)
                return res;
            res = SecondNormalForm();
            if (res.Level == 2)
                return res;
            res = ThirdNormalForm();
            if (res.Level == 3)
                return res;
            res = BcNormalForm();
            if (res.Level == 4)
                return res;
            res.Message = "Mbrouk";
            return res;
        }

        #region 1NF
        private Error FirstNormalForm()
        {
            var res = new Error("",2);
            if (Fd.IsCandidateKey(Fd.CurrentPrimaryKey))
                return res;
            res.Message = "Primary Key isn't candidate key!";
            res.Level = 1;
            var newTable = new BitArray(Fd.Keys.Count);
            newTable.SetAll(true);
            var newKey = Fd.CurrentPrimaryKey;
            foreach (var key in Fd.Left)
            {
                newKey[key] = true;
            }
            foreach (var key in Fd.Right)
            {
                newKey[key] = false;
            }
            foreach (var key in Fd.Middle.Where(key => !Fd.IsPrimeKey(key)))
            {
                newKey[key] = false;
            }
            var clone = newKey;
            if (!Fd.IsSuperKey(newKey))
            {
                foreach (var key in Fd.Middle.Where(key => !newKey[key]))
                {
                    clone[key] = true;
                    if (Fd.IsSuperKey(clone))
                        break;
                }
            }
            foreach (var key in Fd.Middle.Where(key => newKey[key]))
            {
                clone[key] = false;
                clone[key] = !Fd.IsSuperKey(clone);
            }
            newKey = clone;
            res.SuggestedSplit.Add(new Tuple<BitArray, BitArray>(newTable,newKey));
            return res;
        }
        #endregion
        #region 2NF

        private Error SecondNormalForm()
        {
            var res = new Error("",3);
            var newTable = new BitArray(Fd.Keys.Count);
            newTable.SetAll(true);
            foreach (var candidateKey in Fd.SufficientCandidateKeys)
            {
                var clone = candidateKey;
                foreach (var key in Fd.Keys.Where(key => candidateKey[key]))
                {
                    clone[key] = false;
                    var currentCover = Fd.Reachability(clone);
                    if (!Fd.Keys.Any(npKey => newTable[npKey] && !Fd.IsPrimeKey(npKey) && currentCover[npKey]))
                        continue;
                    var Out = Fd.Keys.Where(npKey => newTable[npKey] && !Fd.IsPrimeKey(npKey) && currentCover[npKey]).ToList();
                    foreach (var npKey in Out)
                    {
                        newTable[npKey] = false;
                        var temp = clone;
                        foreach (var rdKey in Fd.Keys.Where(rdKey => clone[rdKey]))
                        {
                            temp[rdKey] = false;
                            if (!Fd.Reachability(temp)[npKey])
                                temp[rdKey] = true;
                        }
                        var index = res.SuggestedSplit.FindIndex(split => split.Item1.Equals(temp));
                        var nextTable = new BitArray(Fd.Keys.Count) {[npKey] = true};
                        if (index != -1)
                        {
                            nextTable.Or(res.SuggestedSplit[index].Item2);
                            res.SuggestedSplit[index] = new Tuple<BitArray, BitArray>(temp,
                                nextTable);
                        }
                        res.SuggestedSplit.Add(new Tuple<BitArray, BitArray>(temp, nextTable));
                    }
                }
            }
            if (res.SuggestedSplit.Count != 0)
            {
                res.SuggestedSplit.Add(new Tuple<BitArray, BitArray>(newTable,Fd.CurrentPrimaryKey));
                res.Message = "Table contains Partial Dependency!";
                res.Level = 2;
            }
            return res;
        }
        #endregion
        #region 3NF
        private Error ThirdNormalForm()
        {
            var res = new Error("",4);
            if (
                Fd.DependencyList.All(
                    dependency =>
                        Fd.IsSuperKey(dependency.Item1) ||
                        Fd.Keys.All(key => !dependency.Item2[key] || Fd.IsPrimeKey(key))))
            {
                return res;
            }
            var table = new BitArray(Fd.Keys.Count);
            table.SetAll(true);
            res.SuggestedSplit.Add(new Tuple<BitArray, BitArray>(table,Fd.CurrentPrimaryKey));
            foreach (var dependency in Fd.DependencyList.Where(dependency => !Fd.IsSuperKey(dependency.Item1) && !Fd.Keys.All(key => !dependency.Item2[key] || Fd.IsPrimeKey(key))))
            {
                var current = new BitArray(Fd.Keys.Count);
                foreach (var key in Fd.Keys)
                {
                    current[key] = table[key] && dependency.Item2[key] && !Fd.IsPrimeKey(key);
                }
                if (!current.ToString().Contains('1'))
                    continue;
                var index = res.SuggestedSplit.FindIndex(newTable => newTable.Item1.Equals(dependency.Item1));
                if(index != -1) { 
                    res.SuggestedSplit[index].Item2.Or(current);
                }else{ 
                    res.SuggestedSplit.Add(new Tuple<BitArray, BitArray>(dependency.Item1,current));
                }
                foreach (var key in Fd.Keys)
                {
                    current[key] = table[key] && (!dependency.Item2[key] || Fd.IsPrimeKey(key));
                }
                table = current;
            }
            res.Message = "Table contains transitive dependency!";
            res.Level = 3;
            return res;
        }
        #endregion
        #region BCNF
        private Error BcNormalForm()
        {
            var res = new Error("", 5);
            if (Fd.DependencyList.All(dependency => Fd.IsSuperKey(dependency.Item1)))
                return res;
            res.Message = "Some attributes doesn't depend on the whole key!";
            res.Level = 4;
            /* 
            * If you wonder why there is no suggestion, read the following:
            * In some cases, a non-BCNF table cannot be decomposed into tables that satisfy BCNF and preserve the dependencies that held in the original table.[BCBP79]
            * Computing a cover for the projection of F on a subset X of R was shown to be inherently exponential in[FJT83].
            * If all dependencies are unary, BCNFTEST can be solved in polynomial time, but BCNFTEST remains EXPTIME-Complete even if a single dependency is binary.[MR89]
            */
            return res;
        }
        #endregion
    }
}
