using System;
using System.Collections;
using System.Linq;

namespace DBNormalizationAnalyzer_AnalyzerLibrary
{
    class NfChecker
    {
        public FunctionalDependency Fd { get; set; }

        public NfChecker(FunctionalDependency fd)
        {
            Fd = fd;
        }

        public Tuple<Error, Suggestion> Check()
        {
            var res = FirstNormalForm();
            if (res.Item1.Level != 0)
                return res;
            res = SecondNormalForm();
            if (res.Item1.Level != 0)
                return res;
            res = ThirdNormalForm();
            if (res.Item1.Level != 0)
                return res;
            res = BcNormalForm();
            if(res.Item1.Level == 0)
                res = new Tuple<Error, Suggestion>(new Error("Mbrouk",0), res.Item2);
            return res;
        }

        #region 1NF
        private Tuple<Error, Suggestion> FirstNormalForm()
        {
            var res = new Tuple<Error, Suggestion>(new Error("", 0), new Suggestion());
            if (Fd.IsCandidateKey(Fd.CurrentPrimaryKey))
                return res;
            var newError = new Error("Primary Key isn't candidate key", 1);
            var newSuggestion = new Suggestion();
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
            res = new Tuple<Error, Suggestion>(newError,newSuggestion);
            return res;
        }
        #endregion
        #region 2NF

        private Tuple<Error, Suggestion> SecondNormalForm()
        {
            var res = new Tuple<Error, Suggestion>(new Error("", 0), new Suggestion());
            var newSuggestion = new Suggestion();
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
                        var index = newSuggestion.SuggestedSplit.FindIndex(split => split.Item1.Equals(temp));
                        var nextTable = new BitArray(Fd.Keys.Count) {[npKey] = true};
                        if (index != -1)
                        {
                            nextTable.Or(newSuggestion.SuggestedSplit[index].Item2);
                            newSuggestion.SuggestedSplit[index] = new Tuple<BitArray, BitArray>(temp,
                                nextTable);
                        }
                        newSuggestion.SuggestedSplit.Add(new Tuple<BitArray, BitArray>(temp, nextTable));
                    }
                }
            }
            if (newSuggestion.SuggestedSplit.Count != 0)
            {
                newSuggestion.SuggestedSplit.Add(new Tuple<BitArray, BitArray>(newTable,Fd.CurrentPrimaryKey));
                var newError = new Error("Table contains Partial Dependency!",2);
                res = new Tuple<Error, Suggestion>(newError,newSuggestion);
            }
            return res;
        }
        #endregion
        #region 3NF
        private Tuple<Error, Suggestion> ThirdNormalForm()
        {
            var res = new Tuple<Error, Suggestion>(new Error("", 0), new Suggestion());
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
            var newSuggestion = new Suggestion();
            newSuggestion.SuggestedSplit.Add(new Tuple<BitArray, BitArray>(table,Fd.CurrentPrimaryKey));
            foreach (var dependency in Fd.DependencyList.Where(dependency => !Fd.IsSuperKey(dependency.Item1) && !Fd.Keys.All(key => !dependency.Item2[key] || Fd.IsPrimeKey(key))))
            {
                var current = new BitArray(Fd.Keys.Count);
                foreach (var key in Fd.Keys)
                {
                    current[key] = table[key] && dependency.Item2[key] && !Fd.IsPrimeKey(key);
                }
                if (!current.ToString().Contains('1'))
                    continue;
                var index = newSuggestion.SuggestedSplit.FindIndex(newTable => newTable.Item1.Equals(dependency.Item1));
                if(index != -1) { 
                    newSuggestion.SuggestedSplit[index] = new Tuple<BitArray, BitArray>(dependency.Item1,
                        newSuggestion.SuggestedSplit[index].Item2.Or(current));
                }else{ 
                    newSuggestion.SuggestedSplit.Add(new Tuple<BitArray, BitArray>(dependency.Item1,current));
                }
                foreach (var key in Fd.Keys)
                {
                    current[key] = table[key] && (!dependency.Item2[key] || Fd.IsPrimeKey(key));
                }
                table = current;
            }
            var newError = new Error("Table contains transitive dependency", 3);
            res = new Tuple<Error, Suggestion>(newError,newSuggestion);
            return res;
        }
        #endregion
        #region BCNF
        private Tuple<Error, Suggestion> BcNormalForm()
        {
            var res = new Tuple<Error, Suggestion>(new Error("", 0), new Suggestion());
            if (Fd.DependencyList.All(dependency => Fd.IsSuperKey(dependency.Item1)))
                return res;
            var newError = new Error("Some attributes doesn't depend on the whole key!", 4);
            res = new Tuple<Error, Suggestion>(newError, new Suggestion());
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
