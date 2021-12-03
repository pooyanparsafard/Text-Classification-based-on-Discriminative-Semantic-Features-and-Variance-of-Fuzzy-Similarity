using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace discriminative
{
    class Functions
    {
        //========== Start of Class

        Variables objectVariables = new Variables();

        //========== Create Matrix Doc-Cat

        public  double[,] createMatrixDocCat(string[] aryDocuments, string[] aryCategories)
        {
                    string labelContainer;
                    double[,] matrixDocCat= new double[aryDocuments.Length,aryCategories.Length];
                    for (int i = 0; i < aryDocuments.Length; i++)
                    {
                            int index=aryDocuments[i].IndexOf("\t");
                            labelContainer = aryDocuments[i].Substring(0,index);
                            for (int j=0 ; j<aryCategories.Length;j++)
                            {
                                if (labelContainer.Contains(aryCategories[j]))
                                {
                                    matrixDocCat[i, j] = 1;
                                }
                            }
                    }
                    return matrixDocCat;

         }

        //========== Omit Label

        public  string[] omitLabel(string[] aryDocuments)
        {
            for (int i = 0; i < aryDocuments.Length; i++)
            {
                int index = aryDocuments[i].IndexOf("\t");
                aryDocuments[i]= aryDocuments[i].Remove(0, index+1);
            }
            return aryDocuments;
        }

        //========== Write Array
        
        //public static void writeArray(string[] array,string StrOutputName )
        //{
        //    string fileName = string.Format("d:\\discriminative\\{0}.txt", StrOutputName);
        //    StreamWriter swArray = new StreamWriter(fileName);
        //    foreach (string i in array)
        //    {
        //        swArray.WriteLine(i);
        //    }
        //    swArray.Close();
        //}

        //========== Remove White Spaces & Non-Alphabeticals

        public  string removeWhiteSpacesAndNonAlphabetical(string oneLineOfArrayDocuments)
        {
            oneLineOfArrayDocuments = oneLineOfArrayDocuments.ToLower();
            oneLineOfArrayDocuments = oneLineOfArrayDocuments.Replace("\n", " ");
            oneLineOfArrayDocuments = Regex.Replace(oneLineOfArrayDocuments, "[^a-zA-Z]+", " ");
            oneLineOfArrayDocuments = Regex.Replace(oneLineOfArrayDocuments, @"\s+", " ");
            return oneLineOfArrayDocuments;
        }

        //========== Remove White Spaces & Non-Alphabeticals (Persian)

        public  string removeWhiteSpacesAndNonAlphabeticalPersian(string oneLineOfArrayDocuments)
        {
            //aryDocuments = aryDocuments.ToLower();
            oneLineOfArrayDocuments = oneLineOfArrayDocuments.Replace("\n", " ");
            oneLineOfArrayDocuments = Regex.Replace(oneLineOfArrayDocuments, "[^آ-ی]+", " ");
            oneLineOfArrayDocuments = Regex.Replace(oneLineOfArrayDocuments, @"\s+", " ");
            return oneLineOfArrayDocuments;
        }

        //========== Write String Array line by line

        public  void writeStringArrayLineByLine (string[] array,string strOutputName)
    {
       string fileName=string.Format("d:\\discriminative\\{0}.txt",strOutputName);
         StreamWriter swStringArrayLineByLine = new StreamWriter(fileName);
            foreach (string i in array)
    {
                swStringArrayLineByLine.WriteLine(i);
    }
        swStringArrayLineByLine.Close();
    }

        //========== Write int Array line by line

        public  void writeIntArrayLineByLine(int[] array, string strOutputName)
        {
            string fileName = string.Format("d:\\discriminative\\{0}.txt", strOutputName);
            StreamWriter swIntArrayLineByLine = new StreamWriter(fileName);
            foreach (int i in array)
            {
                swIntArrayLineByLine.WriteLine(i);
            }
            swIntArrayLineByLine.Close();
        }

        //========== Write Double Array line by line

        public  void writeDoubleArrayLineByLine(double[] array, string strOutputName)
        {
            string fileName = string.Format("d:\\discriminative\\{0}.txt", strOutputName);
            StreamWriter swDoubleArrayLineByLine = new StreamWriter(fileName);
            foreach (double i in array)
            {
                swDoubleArrayLineByLine.WriteLine(i);
            }
            swDoubleArrayLineByLine.Close();
        }

        //========== Extract the Terms

        public  string[] extractTerms(string strDocuments)
        {
            string[] aryTerms;
            strDocuments = strDocuments.Replace("\n", " ");
            strDocuments = Regex.Replace(strDocuments, @"\s+", " ");
            strDocuments = string.Join(" ", strDocuments.Split(' ').Distinct());
            aryTerms = strDocuments.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            return aryTerms;
        }

        //========== Create Matrix Doc-Term

        public  double[,] createMatrixDocTerm(string[] aryDocuments, string[] aryTerms)
        {           
            double[,] matrixDocTerm = new double[aryDocuments.Length,aryTerms.Length];
            for (int i = 0; i < aryDocuments.Length; i++)
            {
                string[] g = aryDocuments[i].Split(' ');
                for (int k = 0; k < g.Length; k++)
                {
                    for (int j = 0; j < aryTerms.Length; j++)
                    {
                        if (g[k] == aryTerms[j])
                        {
                            matrixDocTerm[i, j]++;
                        }
                    }
                }
            }
            return matrixDocTerm;
        }

        //========== Write Matrix

        public  void writeMatrix(double[,] matrixDocTerm, int numberOfDocuments,int numberOfTerms,string strOutputName)
        {
            string fileName = string.Format("d:\\discriminative\\{0}.txt", strOutputName);
            StreamWriter swMatrix = new StreamWriter(fileName);
            for (int i = 0; i < numberOfDocuments; i++)
            {
                for (int j = 0; j < numberOfTerms; j++)
                {
                    swMatrix.Write(matrixDocTerm[i,j]+" ");
                }
                swMatrix.WriteLine();
            }
            swMatrix.Close();
        }

        //========== Write Matrix With Targets

        public  void writeMatrixTargets(double[,] matrixDocTerm,double[,] matrixDocCat, string[] aryCategory, int numberOfDocuments, int numberOfFeatures,int numberOfCategories, string strOutputName, string targets, string ColumnsName)
        {
            string fileName = string.Format("d:\\discriminative\\{0}.txt", strOutputName);
            StreamWriter swMatrixTargets = new StreamWriter(fileName);
            swMatrixTargets.Write("targets ");
            for (int k = 1; k <= numberOfFeatures; k++)
            {
                swMatrixTargets.Write("{0}{1} ",ColumnsName,k);
            }
            swMatrixTargets.WriteLine();

                for (int i = 0; i < numberOfDocuments; i++)
                {
                    for (int z = 0; z < numberOfCategories; z++)
                    {
                        if (matrixDocCat[i, z] == 1)
                            swMatrixTargets.Write("{0} ", aryCategory[z]);
                    }

                    for (int j = 0; j < numberOfFeatures; j++)
                    {
                        swMatrixTargets.Write(matrixDocTerm[i, j] + " ");
                    }
                    swMatrixTargets.WriteLine();
                }
            swMatrixTargets.Close();
        }

        //========== Normalize the Matrix

        public  double[,] normalizMatrix(double[,] matrixDocTerm, int numberOfDocuments , int numberOfTerms)
        {
            
            
            for (int i = 0; i < numberOfDocuments; i++)
            {
                double max = 0;
                double[] arrayMaxFinder = new double[numberOfTerms];

                for (int j = 0; j < numberOfTerms; j++)
                {
                    arrayMaxFinder[j] = matrixDocTerm[i, j];
                }

                max = arrayMaxFinder.Max();

                for (int k = 0; k < numberOfTerms; k++)
                {
                    if (matrixDocTerm[i, k] != 0)
                    {
                        matrixDocTerm[i, k] /= max;
                    }
                }
            }
            return matrixDocTerm;
        }

        //========== Create Matrix Term-Category

        public  double[,] createMatrixTermCat(double[,] matrixDocTerm, double[,] matrixDocCategory,int numberOfterms,int numberOfDocuments, int numberOfCategories)
        {
            double[,] matrixTermCat = new double[numberOfterms,numberOfCategories];
            for (int i = 0; i < numberOfterms; i++)
            {
                for (int k = 0; k < numberOfCategories; k++)
                {
                    double container = 0;
                    for (int j = 0; j < numberOfDocuments; j++)
                    {
                        if (matrixDocTerm[j, i] != 0)
                        {
                            if (matrixDocCategory[j, k] == 1)
                            {
                                container += matrixDocTerm[j, i];
                            }
                        }
                    }
                    matrixTermCat[i, k] = container;
                }
            }
            return matrixTermCat;
        }

        //========== Create Matrix Contingency

        public  double[,] createMatrixContingency(double[,] matrixDocTerm, double[,] matrixDocCat,int numberOfTerms,int numberOfCategories,int numberOfDocs)
        {
            double[,] matrixContingency = new double[numberOfTerms,4*numberOfCategories];
           
               for (int i = 0; i < numberOfTerms; i++)
               {
                   int l = 0;
                       for (int j = 0; j < numberOfCategories*4;j=j+4)
                       {

                           int counter0 = 0; int counter1 = 0; int counter2 = 0; int counter3 = 0; int counterMultiLabelCij = 0;
                           for(int k=0 ; k<numberOfDocs; k++ )
                           {
                               if (matrixDocTerm[k,i]!=0)
                               {
                                   if(matrixDocCat[k,l]==1)
                                   {
                                       counter0++;
                                   }
                               }

                               if (matrixDocTerm[k,i]==0)
                               {
                                   if (matrixDocCat[k,l]==1)
                                   {
                                       counter1++;
                                   }
                               }

                               //---

                               if (matrixDocTerm[k,i]!=0)
                               {
                                   if(matrixDocCat[k,l]==0)
                                   {
                                       counter2++;
                                   }
                               }



                               //if (matrixDocTerm[k, i] != 0)
                               //{
                               //    if (matrixDocCat[k, l] == 0)
                               //    {
                               //        double summer = 0;
                               //        for (int multilabel = 0; multilabel < numberOfCategories; multilabel++)
                               //        {
                               //            summer += matrixDocCat[k, multilabel];
                               //        }
                               //        summer = summer - 1;
                               //        counterMultiLabelCij += Convert.ToInt32(summer);
                               //    }
                               //}

                               //---

                               if (matrixDocTerm[k,i]==0)
                               {
                                   if (matrixDocCat[k,l]==0)
                                   {
                                       counter3++;
                                   }
                               }

                               
                           }
                           matrixContingency[i, j] = counter0;
                           matrixContingency[i, j+1] = counter1;
                           //---
                           matrixContingency[i, j+2] = counter2-counterMultiLabelCij;
                           //---
                           matrixContingency[i, j+3] = counter3;
                           l++;
                   }
                }
               return matrixContingency;
        }

        //========== Create Matrix DFS 

        public  double[,] createMatrixDFS(double[,] matrixContingency, double[,] matrixTermCategory, int numberOfTerms, int numberOfCategories)
        {
            double[,] matrixDFS = new double[numberOfTerms, numberOfCategories];
            for (int i = 0; i < numberOfTerms; i++)
            {
                for (int j = 0; j < numberOfCategories; j++)
                {
                    int aij=j*4;
                    int bij=aij+1;
                    int cij=bij+1;
                    int dij=cij+1;

                    double sumOfCellsOf_ith_RowOfMatrixTermCategory=0;
                    for (int k=0 ; k<numberOfCategories ; k++)
                    {
                        
                        sumOfCellsOf_ith_RowOfMatrixTermCategory += matrixTermCategory[i,k];
                    }
                    double tf__ti__not_cj = sumOfCellsOf_ith_RowOfMatrixTermCategory - matrixTermCategory[i,j];
                    double result2= (tf__ti__not_cj / matrixContingency[i, cij]);
                    if (tf__ti__not_cj == 0 || matrixContingency[i, cij] == 0)
                    {
                        result2 = 1;
                        matrixTermCategory[i, j] = 0;
                    }
                    else
                    {
                        result2 = (tf__ti__not_cj / matrixContingency[i, cij]);
                    }

                    double result1= matrixContingency[i, aij];
                    if (matrixContingency[i, aij] == 0)
                    {
                        result1 = 1;
                    }
                    else
                    {
                        result1 = matrixContingency[i, aij];
                    }

                    matrixDFS[i, j] = ((matrixTermCategory[i, j] / result1) / result2) * ((matrixContingency[i, aij]) / (matrixContingency[i, aij] + matrixContingency[i, bij])) * ((matrixContingency[i, aij]) / (matrixContingency[i, aij] + matrixContingency[i, cij])) * (Math.Abs(((matrixContingency[i, aij]) / (matrixContingency[i, aij] + matrixContingency[i, bij])) - ((matrixContingency[i, cij]) / (matrixContingency[i, cij] + matrixContingency[i, dij]))));
                }
            }
            return matrixDFS;
        }

        //========== Finding Max of each Row of a Matrix and Writing them into the Array

        public  double[] maxOfRowsOfMatrix(double[,] matrix, int numberOfRows , int numberOfColumns)
        {
            double[] aryTermsWithMaxDFS=new double[numberOfRows];
            double[] aryRow=new double[numberOfColumns];

            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    aryRow[j] = matrix[i, j];
                }
                aryTermsWithMaxDFS[i]=aryRow.Max();
            }
            return aryTermsWithMaxDFS;
        }

        //========== Sort Max to Min then Get their Indexes for double Array

        public  int[] sortMaxToMinWithGettingIndexesDoubleArray(double[] array)
        {
            int[] indexes = new int [array.Length];
            int j = 0;
            for (int i = 0; i < array.Length; i++)
            {                
                double max = array.Max();
                indexes[j]=array.ToList().IndexOf(max);
                array[array.ToList().IndexOf(max)] = -1;
                j++;
            }
            return indexes;
        }

        //========== Features with their Indexes

        public  int[] featureIndexes(int[] aryIndexesOfMaxToMinOfFeaturesDFS,int numberOfFeatures)
        {
            int [] Indexes= new int [numberOfFeatures];
            for (int i=0 ; i< numberOfFeatures ; i++)
            {
                Indexes[i]=aryIndexesOfMaxToMinOfFeaturesDFS[i];
            }
            return Indexes;
        }

        //========== Features Selecting (getting input from textbox4) 

        public  string[] featureSelecting(string[] aryTerms,int[] aryIndexes, int numberOfFeaturesToInput)
        {
            string[] features = new string[numberOfFeaturesToInput];
            for (int i = 0; i < numberOfFeaturesToInput; i++)
            {
                int value=aryIndexes[i];
                features[i] = aryTerms[value];
            }
            return features;
        }

        //========== Create Matrix Doc-TFIDF

        public  double[,] createMatrixDocTFIDF(double[,] matrixDocTerm, int[] featureIndexes, int numberOfDocuments)
        {
            double[,] matrixDocTFIDF=new double[numberOfDocuments,featureIndexes.Length];
            objectVariables.aryNm = new int [featureIndexes.Length];

            for (int i=0 ; i<featureIndexes.Length;i++)
            {
                int k = featureIndexes[i]; 
                int counter = 0;
                if (k == -1)
                {
                    objectVariables.aryNm[i] = 0;
                }
                else
                {
                    for (int j = 0; j < numberOfDocuments; j++)
                    {
                        if (matrixDocTerm[j, k] != 0)
                        {
                            counter++;
                        }
                    }
                    objectVariables.aryNm[i] = counter;
                }
            }

            //---

            for (int m = 0; m < numberOfDocuments; m++)
            {
                for (int n = 0; n < featureIndexes.Length; n++)
                {
                    int t=featureIndexes[n];
                    if (t == -1)
                    {
                        matrixDocTFIDF[m, n] = 0;
                    }
                    else
                    {
                        matrixDocTFIDF[m, n] = (matrixDocTerm[m, t]) * (Math.Log10((numberOfDocuments) / (objectVariables.aryNm[n] + 0.01)));
                    }
                }

                double normalization = 0;
                for (int x = 0; x < featureIndexes.Length; x++)
                {
                    normalization += Math.Pow((matrixDocTFIDF[m, x]), 2);
                }

                for (int y = 0; y < featureIndexes.Length; y++)
                {
                    if (normalization == 0)
                    {
                        matrixDocTFIDF[m, y] = 0;
                    }
                    else
                    {
                        matrixDocTFIDF[m, y] = (matrixDocTFIDF[m, y]) / (Math.Sqrt(normalization));
                    }
                }
            }

            //---

            return matrixDocTFIDF;           
         }

        //========== Create Matrix Feature-Feature Similarity

        public  double[,] createMatrixFeatFeatSimilarity(double[,] matrixDocTFIDF, int numberOfFeatures, int numberOfDocuments)
        {
            double[,] matrixFeatFeatSimilarity = new double[numberOfFeatures, numberOfFeatures];
            for (int i = 0; i < numberOfFeatures; i++)
            {
                for (int j = 0; j < numberOfFeatures; j++)
                {

                    double numerator = 0;
                    for (int k = 0; k < numberOfDocuments; k++)
                    {
                        numerator += matrixDocTFIDF[k, i] * matrixDocTFIDF[k, j];
                    }

                    double denominator1 = 0;
                    for (int h = 0; h < numberOfDocuments; h++)
                    {
                        denominator1 += matrixDocTFIDF[h, i] * matrixDocTFIDF[h, i];
                    }

                    double denominator2 = 0;
                    for (int f = 0; f < numberOfDocuments; f++)
                    {
                        denominator2 += matrixDocTFIDF[f, j] * matrixDocTFIDF[f, j];
                    }

                    double denominatorFinal = 0;
                    denominatorFinal=Math.Sqrt(denominator1 * denominator2);

                    if (denominatorFinal == 0)
                    {
                        matrixFeatFeatSimilarity[i, j] = 0;
                    }
                    else
                    {
                        matrixFeatFeatSimilarity[i, j] = numerator / denominatorFinal;
                    }
                }
            }
            return matrixFeatFeatSimilarity;
        }

        //========== Create Matrix Feat-Doc Similarity

        public  double[,] createMatrixFeatDocSimilarity(double[,] matrixDocTFIDF, double[,] matrixFeatFeatSimilarity, int numberOfFeatures, int numberOfDocuments)
        {
            double[,] matrixFeatDocSimilarity = new double[numberOfFeatures,numberOfDocuments];
            for (int i = 0; i < numberOfFeatures; i++)
            {
                for (int j = 0; j < numberOfDocuments; j++)
                {
                    double sim = 0;
                    for (int k=0 ; k<numberOfFeatures ; k++)
                    {
                       sim += matrixDocTFIDF[j, k] * matrixFeatFeatSimilarity[i, k];
                    }
                    matrixFeatDocSimilarity[i,j]=sim;
                }
            }
            return matrixFeatDocSimilarity;
        }

        //========== Create Matrix Doc-TFIDF Modified

        public  double[,] createMatrixDocTFIDFModified(double[,] matrixDocTFIDF, double[,] matrixFeatDocSimilarity,double[,] matrixDocCat, int numberOfDocuments, int numberOfFeatures, int numberOfCategories, bool TestIsSelected)
        {
            // // // //

            //double[,] matrixTermCluster= new double[numberOfFeatures,numberOfCategories];
            //StreamReader srMatrixTermCluster = new StreamReader("d:\\discriminative\\fuzzySimilarity\\matrixTermCluster.txt");
            //for (int t = 0; t < numberOfFeatures; t++)
            //{
            //    string str=srMatrixTermCluster.ReadLine();
            //    string[] ary = str.Split(' ');
            //    for (int tt = 0; tt < numberOfCategories; tt++)
            //    {
            //        matrixTermCluster[t,tt]=Convert.ToDouble(ary[tt]);
            //    }
                
            //}
            //srMatrixTermCluster.Close();

            double[,] matrixTestDocsSimilarToCluster = new double[numberOfDocuments, numberOfCategories];
            if (TestIsSelected)
            {
                StreamReader srMatrixTestDocsSimilarToClusters = new StreamReader("d:\\discriminative\\fuzzySimilarity\\testDocsSimilarToCluster.txt");
                for (int u = 0; u < numberOfDocuments; u++)
                {
                    string str2 = srMatrixTestDocsSimilarToClusters.ReadLine();
                    string[] ary2 = str2.Split(' ');
                    for (int uu = 0; uu < numberOfCategories; uu++)
                    {
                        matrixTestDocsSimilarToCluster[u, uu] = Convert.ToDouble(ary2[uu]);
                    }

                }
                srMatrixTestDocsSimilarToClusters.Close();
            }
            else
            {
                StreamReader srMatrixTestDocsSimilarToClusters2 = new StreamReader("d:\\discriminative\\fuzzySimilarity\\testDocsSimilarToCluster2.txt");
                for (int u2 = 0; u2 < numberOfDocuments; u2++)
                {
                    string str22 = srMatrixTestDocsSimilarToClusters2.ReadLine();
                    string[] ary22 = str22.Split(' ');
                    for (int uu2 = 0; uu2 < numberOfCategories; uu2++)
                    {
                        matrixTestDocsSimilarToCluster[u2, uu2] = Convert.ToDouble(ary22[uu2]);
                    }

                }
                srMatrixTestDocsSimilarToClusters2.Close();

                //for (int g = 0; g < numberOfDocuments; g++)
                //{
                //    for (int gg = 0; gg < numberOfCategories; gg++)
                //    {
                //        matrixTestDocsSimilarToCluster[g, gg] = matrixDocCat[g, gg];
                //    }
                //}
            }


            // // // // 

           double[,] matrixDocTFIDFModified = new double[numberOfDocuments,numberOfFeatures];
           for (int i = 0; i < numberOfDocuments; i++)
           {
                   double simDenominator=0;
                   for (int k=0 ; k<numberOfFeatures ; k++)
                   {
                       simDenominator+= matrixDocTFIDF[i,k];
                   }

                   for (int j = 0; j < numberOfFeatures; j++)
                   {
                       if (simDenominator == 0)
                       {
                           // // // //
                           double newValue2 = 0;
                           for (int qq = 0; qq < numberOfCategories; qq++)
                           {
                               newValue2 += /*matrixTermCluster[j, qq] * */matrixTestDocsSimilarToCluster[i, qq];
                           }
                           matrixDocTFIDFModified[i, j] = matrixDocTFIDF[i, j] + (newValue2);
                           // // // //
                       }
                       else
                       {
                           // // // //
                           double newValue=0;
                           for (int q=0 ; q<numberOfCategories;q++)
                           {
                                newValue += /*matrixTermCluster[j,q]* */ matrixTestDocsSimilarToCluster[i,q];
                           }

                           matrixDocTFIDFModified[i, j] = matrixDocTFIDF[i, j] + (matrixFeatDocSimilarity[j, i] / simDenominator) + newValue;
                           // // // //
                       }
                   }
           }
           return matrixDocTFIDFModified;
        }

        //========== Round the Matrix

        public  void roundMatrix(double[,] matrix, int numberOfRows, int numberOfColumns)
        {
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    matrix[i, j] = Math.Round(matrix[i, j], 4, MidpointRounding.AwayFromZero);
                }
            }
        }

        //==========  MultiLabels Counting

        public int multiLabelsCounting(double[,] matrixDocCat, int numberOfDocuments, int numberOfCategories)
        {
            int numberOfTrainMultiLabels = 0;
            for (int numberOfDocs = 0; numberOfDocs < numberOfDocuments; numberOfDocs++)
            {
                int multiLabelCounter = 0;
                for (int numberOfCats = 0; numberOfCats < numberOfCategories; numberOfCats++)
                {
                    if (matrixDocCat[numberOfDocs, numberOfCats] == 1)
                        multiLabelCounter++;
                }
                if (multiLabelCounter > 1)
                    numberOfTrainMultiLabels++;
            }
            return numberOfTrainMultiLabels;
        }

        //==========  Labels Counting

        public int labelsCounting(double[,] matrixDocCat, int numberOfDocuments, int numberOfCategories)
        {
            int numberOfLabels = 0;
            for (int numberOfDocs = 0; numberOfDocs < numberOfDocuments; numberOfDocs++)
            {
                for (int numberOfCats = 0; numberOfCats < numberOfCategories; numberOfCats++)
                {
                    if (matrixDocCat[numberOfDocs, numberOfCats] == 1)
                    {
                        numberOfLabels++;
                    }
                }
            }
            return numberOfLabels;
        }

        //========== Ready the Train Array to Next Application (Train)

        public void readyTrainArrayNextApplication(string[] aryTextLineByLine,string[] aryFeature,string[] aryCategory,double[,] matrixDocCat,int numberOfCategories,int numberOfTrainLabels,string strOutputName, string strOutputName2 )
        {
            string fileName = string.Format("d:\\discriminative\\fuzzySimilarity\\{0}.txt", strOutputName);
            string fileName2 = string.Format("d:\\discriminative\\fuzzySimilarity\\{0}.txt", strOutputName2);
            StreamWriter sw = new StreamWriter(fileName);
            StreamWriter sw2 = new StreamWriter(fileName2);
            int labelCounter = 0;
            for (int i = 0; i < aryTextLineByLine.Length; i++)
            {
                string[] oneLineArray = aryTextLineByLine[i].Split(' ');
                
                for (int y = 0; y < numberOfCategories; y++)
                {
                    if (matrixDocCat[i, y] == 1)
                    {
                        sw.Write("{0} ", aryCategory[y]);
                        
                        labelCounter++;

                        if (labelCounter != numberOfTrainLabels)
                        {
                            sw2.WriteLine(y + 1);
                        }
                        else
                        {
                            sw2.Write(y+1);
                        }

                        sw.Write("\t");


                        for (int j = 0; j < oneLineArray.Length; j++)
                        {
                            for (int k = 0; k < aryFeature.Length; k++)
                            {
                                if (oneLineArray[j] == aryFeature[k])
                                    sw.Write(oneLineArray[j] + " ");
                            }
                        }
                        if ( labelCounter != numberOfTrainLabels)
                            sw.Write("\n");
                    }
                  }
            }
            sw.Close();
            sw2.Close();
        }

        //========== Ready the Test Array to Next Application (Test)

        public void readyTestArrayNextApplication(string[] aryTextLineByLine, string[] aryFeature, string[] aryCategory, double[,] matrixDocCat, int numberOfCategories , string strOutputName)
        {
            string fileName = string.Format("d:\\discriminative\\fuzzySimilarity\\{0}.txt", strOutputName);
            StreamWriter sw = new StreamWriter(fileName);
            for (int i = 0; i < aryTextLineByLine.Length; i++)
            {
                string[] oneLineArray = aryTextLineByLine[i].Split(' ');

                bool greaterThanOne = false;
                for (int y = 0; y < numberOfCategories; y++)
                {
                    if (matrixDocCat[i, y] == 1 && greaterThanOne==false)
                    {
                        sw.Write("{0}", aryCategory[y]);
                        greaterThanOne = true;
                    }
                }
                sw.Write("\t");


                for (int j = 0; j < oneLineArray.Length; j++)
                {
                    for (int k = 0; k < aryFeature.Length; k++)
                    {
                        if (oneLineArray[j] == aryFeature[k])
                            sw.Write(oneLineArray[j] + " ");
                    }
                }
                if (i != aryTextLineByLine.Length - 1)
                    sw.Write("\n");
            }
            sw.Close();
        }

        //========== 01 

        public void zeroOne(double[,] matrixDocCat, int numberOfDocuments, int numberOfCategories)
        {
            StreamWriter sw = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\01.txt");
            
            for (int numberOfDocs = 0; numberOfDocs < numberOfDocuments; numberOfDocs++)
            {
                int Counter = 0;
                for (int numberOfCats = 0; numberOfCats < numberOfCategories; numberOfCats++)
                {
                    if (matrixDocCat[numberOfDocs, numberOfCats] == 1)
                    {
                        Counter++;
                    }
                }

                if (Counter > 1)
                {
                    if (numberOfDocs != numberOfDocuments - 1)
                    {
                        sw.WriteLine("1");
                    }
                    else
                    {
                        sw.Write("1");
                    }
                }
                else
                {
                    if (numberOfDocs != numberOfDocuments - 1)
                    {
                        sw.WriteLine("0");
                    }
                    else
                    {
                        sw.Write("0");
                    }
                }
            }
            sw.Close();
        }

        //==========

        //========== End of Class
    }
}
