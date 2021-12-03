using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;



namespace ConsoleApplication25
{

    class Program
    {
        //==================================================== START ================================================================================

        //========== CLASSES & FUNCTIONS

        static void Main(string[] args)
        {

            //******************* PART 1 - GETTING TEXTS TERM EXTRACTION & CREATING DOC-TERM MATRIX *************************************************************

            //-------------------------- RULES ------------------------------------------------------------------------------------------------------------------

            // 1.These are input Files:
            //   a.titleText.txt (if available no need to input (b))
            //   b.text.txt (if (a) is not available, need to input but should comment (OPTIONAL 1) Part)
            //   c.clusterID.txt
            //   d.evaluationTitleText (if available no need to input (e))
            //   e.evaluationText (if (d) is not available, need to input but should comment (OPTIONAL 2) Part)
            //   f.evaluationClusterID (just in a special case no need to input, that is IDEAL CLUSTERING else should comment (OPTIONAL 3))
            //   g.HANDY INPUTS (m=dimension)
            //
            // 2.Do not forget to OMIT the last \n from:
            //   (a)
            //   (b) if input handy need to omit \n, else automatically is true condition(when (a) input no need to omit)
            //   (d)
            //   (e) if input handy need to omit \n, else automatically is true condition(when (d) input no need to omit)
            //
            // 3.Do not forget to choose a unic method for both of Md(t)`s & Term Extracting Operation. 
            //
            // 4.

            //========== GETTING HANDY INPUTS        

            double m = 2;
            int Alpha = 110;


            //========== CONVERTING THE TITLE TEXT TO TEXT === (OPTIONAL 1) IT`S FOR REUTERS DATASET

            StreamReader srDocNoTitleText = new StreamReader("d:\\discriminative\\fuzzySimilarity\\textTitle.txt");
            string strDocNoTitleText = srDocNoTitleText.ReadToEnd();
            string[] aryDocNoTitleText = strDocNoTitleText.Split('\n');
            int DocNoTitleText = aryDocNoTitleText.Length;
            srDocNoTitleText.Close();


            StreamReader srTextTitle = new StreamReader("d:\\discriminative\\fuzzySimilarity\\textTitle.txt");

            string strOmitTitleText = srTextTitle.ReadLine();
            int s = 0;
            string[] omitting = new string[DocNoTitleText];

            for (int i = 0; i < DocNoTitleText; i++)
            {
                string strLine = "";

                strOmitTitleText = strOmitTitleText.Replace("\t", " ");
                strOmitTitleText = strOmitTitleText.Replace("\n", " ");
                strOmitTitleText = strOmitTitleText.Replace("\r", " ");
                strOmitTitleText = strOmitTitleText.Replace("\b", " ");
                strOmitTitleText = strOmitTitleText.Replace("\f", " ");
                strOmitTitleText = strOmitTitleText.Replace("\n", " ");
                strOmitTitleText = strOmitTitleText.Replace("\v", " ");
                strOmitTitleText = strOmitTitleText.Replace("\a", " ");
                strOmitTitleText = strOmitTitleText.Replace("\0", " ");
                strOmitTitleText = strOmitTitleText.Replace("\\", " ");
                strOmitTitleText = strOmitTitleText.Replace("\'", " ");
                strOmitTitleText = strOmitTitleText.Replace("\"", " ");

                string[] aryOmitTitleText = strOmitTitleText.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                List<string> lstOmitTitleText = new List<string>(aryOmitTitleText);
                lstOmitTitleText.RemoveAt(0);

                for (int b = 0; b < lstOmitTitleText.Count; b++)
                {
                    strLine = strLine + lstOmitTitleText[b] + " ";
                }
                omitting[s] = strLine;
                s++;
                strOmitTitleText = srTextTitle.ReadLine();
            }
            srTextTitle.Close();

            //++++++++++

            StreamWriter swTextTitle = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\text.txt");
            foreach (string i in omitting)
            {
                swTextTitle.WriteLine(i);
            }
            swTextTitle.Close();

            //========== GETTING TEXT & TERM EXTRACTION (also can to be input)=(IMPORTANT if inputed handy need to conver .lentgh-1 to .lenght)

            StreamReader srText = new StreamReader("d:\\discriminative\\fuzzySimilarity\\text.txt");
            string strText = srText.ReadToEnd();
            srText.Close();

            string[] docNoStr = strText.Split('\n');
            int docNo = docNoStr.Length - 1;   // previous write one extra \n Added to last line

            strText = strText.Replace("\t", " ");
            strText = strText.Replace("\n", " ");
            strText = strText.Replace("\r", " ");
            strText = strText.Replace("\b", " ");
            strText = strText.Replace("\f", " ");
            strText = strText.Replace("\n", " ");
            strText = strText.Replace("\v", " ");
            strText = strText.Replace("\a", " ");
            strText = strText.Replace("\0", " ");
            strText = strText.Replace("\\", " ");
            strText = strText.Replace("\'", " ");
            strText = strText.Replace("\"", " ");

            strText = string.Join(" ", strText.Split(' ').Distinct());

            string[] strAry = strText.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            List<string> lst = new List<string>(strAry);

            lst.Sort();

            int termNo = lst.Count();

            //++++++++++

            StreamWriter swLst = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\termList.txt");
            string strLst;
            for (int i = 0; i < termNo; i++)
            {
                strLst = lst[i];
                swLst.WriteLine(strLst);
            }
            swLst.Close();

            //========== CREATING THE DOC-TERM MATRIX 

            StreamReader srMatrixDocTerm = new StreamReader("d:\\discriminative\\fuzzySimilarity\\text.txt");
            string strMatrixDocTerm = srMatrixDocTerm.ReadLine();

            float[,] matrixDocTerm = new float[docNo, termNo];

            string[] aryDocTerm;
            int col;
            int row = 0;
            while (strMatrixDocTerm != null) //(row < docNo)
            {
                strMatrixDocTerm = strMatrixDocTerm.Replace("\t", " ");
                strMatrixDocTerm = strMatrixDocTerm.Replace("\n", " ");
                strMatrixDocTerm = strMatrixDocTerm.Replace("\r", " ");
                strMatrixDocTerm = strMatrixDocTerm.Replace("\b", " ");
                strMatrixDocTerm = strMatrixDocTerm.Replace("\f", " ");
                strMatrixDocTerm = strMatrixDocTerm.Replace("\n", " ");
                strMatrixDocTerm = strMatrixDocTerm.Replace("\v", " ");
                strMatrixDocTerm = strMatrixDocTerm.Replace("\a", " ");
                strMatrixDocTerm = strMatrixDocTerm.Replace("\0", " ");
                strMatrixDocTerm = strMatrixDocTerm.Replace("\\", " ");
                strMatrixDocTerm = strMatrixDocTerm.Replace("\'", " ");
                strMatrixDocTerm = strMatrixDocTerm.Replace("\"", " ");
                aryDocTerm = strMatrixDocTerm.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < aryDocTerm.Length; i++)
                {
                    col = lst.IndexOf(aryDocTerm[i]);
                    matrixDocTerm[row, col]++;
                }
                row++;
                strMatrixDocTerm = srMatrixDocTerm.ReadLine();
            }
            srMatrixDocTerm.Close();

            //==========

            //---------- Paper Method = TF - if three others method get comment , this method is ACTIVE by default:

            //                        by default is ACTIVE

            //---------- Normalization Method = W / MAX

            float[] maxOfEachTermCuntOfEachDocFromMatrixDocTerm = new float[docNo];
            float[] valueOfEachCellsOfEachDocsOfMatrixDocTerm = new float[termNo];
            for (int i = 0; i < docNo; i++)
            {
                for (int j = 0; j < termNo; j++)
                {
                    valueOfEachCellsOfEachDocsOfMatrixDocTerm[j] = matrixDocTerm[i, j];
                }
                maxOfEachTermCuntOfEachDocFromMatrixDocTerm[i] = valueOfEachCellsOfEachDocsOfMatrixDocTerm.Max();
            }


            for (int i = 0; i < docNo; i++)
            {
                for (int j = 0; j < termNo; j++)
                {
                    if (matrixDocTerm[i, j] != 0)
                    {
                        matrixDocTerm[i, j] = matrixDocTerm[i, j] / maxOfEachTermCuntOfEachDocFromMatrixDocTerm[i];
                    }
                }
            }

            //---------- Binary Method 

            //for (int i = 0; i < docNo; i++)
            //{
            //    for (int j = 0; j < termNo; j++)
            //    {
            //        if (matrixDocTerm[i, j] != 0)
            //        {
            //            matrixDocTerm[i, j] = 1;
            //        }
            //    }
            //}

            //---------- Membership Method = W / All(Count)

            //float[] sumOfTermsOfEachDocOfmatrixDocTerm = new float[docNo];
            //for (int i = 0; i < docNo; i++)
            //{
            //    float sumValueOfCellsOfEachDocsOfMatrixDocTerm = 0;
            //    for (int j = 0; j < termNo; j++)
            //    {
            //        sumValueOfCellsOfEachDocsOfMatrixDocTerm += matrixDocTerm[i, j];
            //    }
            //    sumOfTermsOfEachDocOfmatrixDocTerm[i] = sumValueOfCellsOfEachDocsOfMatrixDocTerm;
            //}


            //for (int i = 0; i < docNo; i++)
            //{
            //    for (int j = 0; j < termNo; j++)
            //    {
            //        if (matrixDocTerm[i, j] != 0)
            //        {
            //            matrixDocTerm[i, j] /= sumOfTermsOfEachDocOfmatrixDocTerm[i];
            //        }
            //    }
            //}

            //++++++++++

            StreamWriter swMatrixDocTerm = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\matrixDocTerm.txt");
            for (int i = 0; i < docNo; i++)
            {
                for (int j = 0; j < termNo; j++)
                {
                    swMatrixDocTerm.Write(matrixDocTerm[i, j] + " ");
                }
                swMatrixDocTerm.WriteLine();
            }
            swMatrixDocTerm.Close();

            //System.Console.WriteLine(matrixDocTerm[22, 1556]);

            //******************** PART 2 - GETTING CLUSTERID & CREATING DOC-CLUSTER MATRIX & PROBABILITY MATRIX ************************************************  

            //========== GETTING CLUSTERID ARRAY

            StreamReader srClusterID = new StreamReader("d:\\discriminative\\fuzzySimilarity\\clusterID.txt");
            int[] aryCluster = new int[docNo];
            for (int i = 0; i < docNo; i++)
            {
                aryCluster[i] = System.Convert.ToInt32(srClusterID.ReadLine());
            }
            srClusterID.Close();

            int clusterNo = aryCluster.Max();

            for (int i = 0; i < docNo; i++)
            {
                aryCluster[i] = aryCluster[i] - 1;
            }

            //========== CREATING TERM-CLUSTER MATRIX 

            float[,] matrixTermCluster = new float[termNo, clusterNo];


            for (int c = 0; c < clusterNo; c++)
            {

                for (int j = 0; j < termNo; j++)
                {
                    float valueMatrixTermCluster = 0;
                    for (int i = 0; i < docNo; i++)
                    {

                        if (matrixDocTerm[i, j] != 0 && aryCluster[i] == c)
                        {
                            valueMatrixTermCluster = matrixDocTerm[i, j] + valueMatrixTermCluster;
                        }

                    }
                    matrixTermCluster[j, c] = valueMatrixTermCluster;
                }
            }

            //++++++++++

            StreamWriter swMatrixTermCluster = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\matrixTermCluster.txt");
            for (int j = 0; j < termNo; j++)
            {
                // // // // //
                float valueSummation = 0;
                for (int ala = 0; ala < clusterNo; ala++)
                {
                    valueSummation += matrixTermCluster[j, ala];
                }

                    // // // // //
                    for (int c = 0; c < clusterNo; c++)
                    {
                        if (matrixTermCluster[j, c] != 0)
                        {
                            swMatrixTermCluster.Write(matrixTermCluster[j, c]/valueSummation + " ");
                        }
                        else
                        {
                            swMatrixTermCluster.Write(matrixTermCluster[j, c] + " ");
                        }
                    }
                swMatrixTermCluster.WriteLine();
            }
            swMatrixTermCluster.Close();


            //========== CREATING PROBABILITY MATRIX 

            float[,] matrixProbability = new float[termNo, clusterNo];


            for (int j = 0; j < termNo; j++)
            {
                float sum = 0;
                for (int c = 0; c < clusterNo; c++)
                {
                    sum = sum + matrixTermCluster[j, c];
                }

                for (int c = 0; c < clusterNo; c++)
                {
                    matrixProbability[j, c] = matrixTermCluster[j, c] / sum;
                }

            }

            //for (int numOfTerms = 0; numOfTerms < termNo; numOfTerms++)
            //{
            //    float maxOfMatrixProbabilityRow = 0;
            //    for (int numOfCategories = 0; numOfCategories < clusterNo; numOfCategories++)
            //    {
            //        if (matrixProbability[numOfTerms, numOfCategories] > maxOfMatrixProbabilityRow)
            //        {
            //            maxOfMatrixProbabilityRow = matrixProbability[numOfTerms, numOfCategories];
            //        }
            //    }

            //    for (int numOfCategories = 0; numOfCategories < clusterNo; numOfCategories++)
            //    {
            //        if (maxOfMatrixProbabilityRow != 0)
            //            matrixProbability[numOfTerms, numOfCategories] /= maxOfMatrixProbabilityRow;
            //    }
            //}

            //++++++++++

            StreamWriter swProbabilityMatrix = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\matrixProbability.txt");

            for (int j = 0; j < termNo; j++)
            {
                for (int c = 0; c < clusterNo; c++)
                {
                    swProbabilityMatrix.Write(matrixProbability[j, c] + " ");
                }
                swProbabilityMatrix.WriteLine();
            }
            swProbabilityMatrix.Close();



            //******************* PART 3 - GETTING THE TEST DOCUMENT AND COMPUTING THE SIMILARITY ***************************************************************

            //========== GETTING THE TEST DOCUMENT & CREATING THE TESTDOCTERMLIST 

            StreamReader srSpecificTermListTestDoc = new StreamReader("d:\\discriminative\\fuzzySimilarity\\test.txt");
            string strSpecificTestDoc = srSpecificTermListTestDoc.ReadToEnd();
            srSpecificTermListTestDoc.Close();

            strSpecificTestDoc = strSpecificTestDoc.Replace("\t", " ");
            strSpecificTestDoc = strSpecificTestDoc.Replace("\n", " ");
            strSpecificTestDoc = strSpecificTestDoc.Replace("\r", " ");
            strSpecificTestDoc = strSpecificTestDoc.Replace("\b", " ");
            strSpecificTestDoc = strSpecificTestDoc.Replace("\f", " ");
            strSpecificTestDoc = strSpecificTestDoc.Replace("\n", " ");
            strSpecificTestDoc = strSpecificTestDoc.Replace("\v", " ");
            strSpecificTestDoc = strSpecificTestDoc.Replace("\a", " ");
            strSpecificTestDoc = strSpecificTestDoc.Replace("\0", " ");
            strSpecificTestDoc = strSpecificTestDoc.Replace("\\", " ");
            strSpecificTestDoc = strSpecificTestDoc.Replace("\'", " ");
            strSpecificTestDoc = strSpecificTestDoc.Replace("\"", " ");
            string[] aryAllTermTestDoc = strSpecificTestDoc.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            strSpecificTestDoc = string.Join(" ", strSpecificTestDoc.Split(' ').Distinct());

            string[] arySpecificTermTestDoc = strSpecificTestDoc.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            Array.Sort(aryAllTermTestDoc);
            Array.Sort(arySpecificTermTestDoc);
            int TermNoSpecificTestDoc = arySpecificTermTestDoc.Length;
            int TermNoAllTestDoc = aryAllTermTestDoc.Length;

            //++++++++++

            StreamWriter swSpecificTermListTestDoc = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\testDocSpecificTermList.txt");

            foreach (string i in arySpecificTermTestDoc)
            {
                swSpecificTermListTestDoc.WriteLine(i);
            }
            swSpecificTermListTestDoc.Close();

            //========== TERM COUNTER OF TEST DOCUMENT 

            float[] TermSpecificCounterOfSpecificTermList = new float[TermNoSpecificTestDoc];

            for (int i = 0; i < TermNoSpecificTestDoc; i++)
            {
                float strCounter = 0;
                for (int j = 0; j < TermNoAllTestDoc; j++)
                {
                    if (arySpecificTermTestDoc[i] == aryAllTermTestDoc[j])
                    {
                        strCounter = strCounter + 1;
                    }
                }
                TermSpecificCounterOfSpecificTermList[i] = strCounter;
            }

            //++++++++++

            StreamWriter swSpecificTermCounter = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\testDocSpecificTermCounter.txt");

            foreach (float i in TermSpecificCounterOfSpecificTermList)
            {
                swSpecificTermCounter.WriteLine(i);
            }
            swSpecificTermCounter.Close();


            //========== TEST VECTOR

            float[] testVector = new float[termNo];

            for (int i = 0; i < termNo; i++)
            {
                float valueTestVector = 0;
                for (int j = 0; j < TermNoAllTestDoc; j++)
                {
                    if (lst[i] == aryAllTermTestDoc[j])
                    {
                        valueTestVector = valueTestVector + 1;

                    }
                }
                testVector[i] = valueTestVector;
            }

            //==========

            //---------- Paper Method = TF - if three others method get comment , this method is ACTIVE by default:

            //           by default is ACTIVE

            //---------- Normalization Method = W / MAX

            float maxOfTestVector = testVector.Max();
            for (int i = 0; i < termNo; i++)
            {
                if (testVector[i] != 0)
                {
                    testVector[i] /= maxOfTestVector;
                }
            }

            //---------- Binary Method 

            //for (int i = 0; i < termNo; i++)
            //{
            //    if (testVector[i] != 0)
            //    {
            //        testVector[i] = 1;
            //    }
            //}

            //---------- Membership Method = W / All(Count)

            //float sumOfTestVector = 0;
            //for (int i = 0; i < termNo; i++)
            //{
            //    sumOfTestVector += testVector[i];
            //}


            //for (int i = 0; i < termNo; i++)
            //{
            //    if (testVector[i] != 0)
            //    {
            //        testVector[i] /= sumOfTestVector;
            //    }
            //}

            //++++++++++

            StreamWriter swTestVector = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\testVector.txt");
            for (int i = 0; i < termNo; i++)
            {
                swTestVector.WriteLine(testVector[i]);
            }
            swTestVector.Close();

            ////========== Md(t)

            float[] MooDT = new float[termNo];
            Array.Copy(testVector, MooDT, termNo);

            ////////////////////////////////////////////---------- Normalization = Paper Method = W / W max 

            ////////////////////////////////////////////float MooDTDenominator = TermSpecificCounterOfSpecificTermList.Max();
            ////////////////////////////////////////////float[] MooDT = new float[TermNoSpecificTestDoc];

            ////////////////////////////////////////////for (int i = 0; i < TermNoSpecificTestDoc; i++)
            ////////////////////////////////////////////{
            ////////////////////////////////////////////    MooDT[i] = TermSpecificCounterOfSpecificTermList[i] / MooDTDenominator;
            ////////////////////////////////////////////}

            ////////////////////////////////////////////---------- Binary Method

            //////////////////////////////////////////int[] MooDT = new int[TermNoSpecificTestDoc];

            //////////////////////////////////////////for (int i = 0; i < TermNoSpecificTestDoc; i++)
            //////////////////////////////////////////{
            //////////////////////////////////////////    MooDT[i] = 1;
            //////////////////////////////////////////}

            ////////////////////////////////////////////---------- Membership Method W / All Test W

            //////////////////////////////////////////int MooDTDenominator = aryAllTermTestDoc.Count();
            //////////////////////////////////////////float[] MooDT = new float[TermNoSpecificTestDoc];

            //////////////////////////////////////////for (int i = 0; i < TermNoSpecificTestDoc; i++)
            //////////////////////////////////////////{
            //////////////////////////////////////////    MooDT[i] = TermSpecificCounterOfSpecificTermList[i] / MooDTDenominator;
            //////////////////////////////////////////}

            //++++++++++

            StreamWriter swMooDT = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\MooDT.txt");

            foreach (float i in MooDT)
            {
                swMooDT.WriteLine(i);
            }
            swMooDT.Close();

            //========== CREATING THE UPSIM & DOWNSIM MATRIXES

            float value2, downSim, upSim;
            float[] aryUpSim = new float[TermNoSpecificTestDoc];
            float[] aryDownSim = new float[TermNoSpecificTestDoc];
            float[,] matrixUpSim = new float[TermNoSpecificTestDoc, clusterNo];
            float[,] matrixDownSim = new float[TermNoSpecificTestDoc, clusterNo];

            for (int c = 0; c < clusterNo; c++)
            {
                for (int i = 0; i < /*termNo*/ TermNoSpecificTestDoc; i++)
                {
                    for (int j = 0; j < termNo; j++)
                    {
                        if (arySpecificTermTestDoc[i] == lst[j])
                        {
                            upSim = matrixProbability[j, c] * MooDT[j];
                            value2 = matrixProbability[j, c] + MooDT[j];
                            downSim = value2 - upSim;

                            aryUpSim[i] = upSim;
                            aryDownSim[i] = downSim;
                            matrixUpSim[i, c] = upSim;
                            matrixDownSim[i, c] = downSim;
                        }
                    }
                }
            }

            //----------

            StreamWriter swMatrixUpSim = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\matrixUpSim.txt");

            for (int i = 0; i < TermNoSpecificTestDoc; i++)
            {
                for (int c = 0; c < clusterNo; c++)
                {
                    swMatrixUpSim.Write(matrixUpSim[i, c] + " ");
                }
                swMatrixUpSim.WriteLine();
            }
            swMatrixUpSim.Close();



            StreamWriter swMatrixDownSim = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\matrixDownSim.txt");

            for (int i = 0; i < TermNoSpecificTestDoc; i++)
            {
                for (int c = 0; c < clusterNo; c++)
                {
                    swMatrixDownSim.Write(matrixDownSim[i, c] + " ");
                }
                swMatrixDownSim.WriteLine();
            }
            swMatrixDownSim.Close();

            //========== SIMILARITY OF TEST DOC TO CLUSTERS COMPUTATION 

            float[] aryUpSummations = new float[clusterNo];
            float[] aryDownSummations = new float[clusterNo];
            float upSummations, downSummations;
            for (int c = 0; c < clusterNo; c++)
            {
                upSummations = 0;
                for (int i = 0; i < TermNoSpecificTestDoc; i++)
                {
                    upSummations = matrixUpSim[i, c] + upSummations;
                }
                aryUpSummations[c] = upSummations;
            }


            for (int c = 0; c < clusterNo; c++)
            {
                downSummations = 0;
                for (int i = 0; i < TermNoSpecificTestDoc; i++)
                {
                    downSummations = matrixDownSim[i, c] + downSummations;
                }
                aryDownSummations[c] = downSummations;
            }


            float[] arySimilarity = new float[clusterNo];
            for (int c = 0; c < clusterNo; c++)
            {
                arySimilarity[c] = aryUpSummations[c] / aryDownSummations[c];
            }

            //++++++++++

            StreamWriter swArySimilarity = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\clustersSimilarities.txt");
            foreach (float c in arySimilarity)
            {
                swArySimilarity.WriteLine(c);
            }
            swArySimilarity.WriteLine();
            swArySimilarity.WriteLine();
            swArySimilarity.WriteLine();
            swArySimilarity.WriteLine();

            float[] aryTopTwoSimilarity = new float[clusterNo];
            Array.Copy(arySimilarity, aryTopTwoSimilarity, clusterNo);
            Array.Sort(aryTopTwoSimilarity);

            int candidateCluster1 = 0;
            int candidateCluster2 = 0;
            for (int c = 0; c < clusterNo; c++)
            {
                if (aryTopTwoSimilarity[clusterNo - 1] == arySimilarity[c])
                {
                    candidateCluster1 = c;
                }
                if (aryTopTwoSimilarity[clusterNo - 2] == arySimilarity[c])
                {
                    candidateCluster2 = c;
                }
            }

            swArySimilarity.WriteLine("Document can belong to Candidate Clusters Number {0} & {1} with {2} and {3} value of Similarity.", candidateCluster1 + 1, candidateCluster2 + 1, aryTopTwoSimilarity[clusterNo - 1], aryTopTwoSimilarity[clusterNo - 2]);
            swArySimilarity.Close();

            //========== SIMILARITY OF TEST DOC TO CANDIDATES DOCS COMPUTATION 

            int candidateCounter = 0;

            for (int i = 0; i < docNo; i++)
            {
                if (aryCluster[i] == candidateCluster1 || aryCluster[i] == candidateCluster2)
                    candidateCounter = candidateCounter + 1;
            }
            int docNoCandidated = candidateCounter;



            int[] document = new int[docNoCandidated];
            double[] similaritiesOfTestDocToCandidateDocs = new double[docNoCandidated];
            int q = 0;
            double oneOnM = 1 / m;

            for (int i = 0; i < docNo; i++)
            {
                double summerOfDocsOfCandidatesWithTestDoc = 0;

                if (aryCluster[i] == candidateCluster1 || aryCluster[i] == candidateCluster2)
                {
                    for (int j = 0; j < termNo; j++)
                    {
                        document[q] = i;

                        summerOfDocsOfCandidatesWithTestDoc = (Math.Abs(matrixDocTerm[i, j] - testVector[j]) * Math.Abs(matrixDocTerm[i, j] - testVector[j])) + summerOfDocsOfCandidatesWithTestDoc;
                    }
                    double summerOfDocsOfCandidatesWithTestDocOnM = summerOfDocsOfCandidatesWithTestDoc / m;
                    similaritiesOfTestDocToCandidateDocs[q] = Math.Pow(summerOfDocsOfCandidatesWithTestDocOnM, oneOnM);
                    q++;

                }

            }

            //----------

            StreamWriter swSimilaritiesOfTestDocToCandidateDocs = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\testDocSimilarities.txt");
            for (int i = 0; i < docNoCandidated; i++)
            {
                swSimilaritiesOfTestDocToCandidateDocs.WriteLine("{0}-{1}", document[i], similaritiesOfTestDocToCandidateDocs[i]);
            }
            swSimilaritiesOfTestDocToCandidateDocs.Close();

            //******************************************* PART 3 - THE EVALUATION *******************************************************************************

            //---------------------------------------------- GETTING INPUTS -------------------------------------------------------------------------------------

            int docNoEvaluation;

            //========== EVALUATION titleText DocNo === (OPTIONAL 2)===(if commented has an IMPORTANT in EVALUATION Text DocNo )

            StreamReader srDocNoEvaluationTitleText = new StreamReader("d:\\discriminative\\fuzzySimilarity\\evaluationTitleText.txt");
            string strDocNoEvaluationTitleText = srDocNoEvaluationTitleText.ReadToEnd();
            string[] aryDocNoEvaluationTitleText = strDocNoEvaluationTitleText.Split('\n');
            docNoEvaluation = aryDocNoEvaluationTitleText.Length;
            srDocNoEvaluationTitleText.Close();

            //========== GETTING evaluationTitleText === (OPTIONAL 2)

            StreamReader srEvaluationText = new StreamReader("d:\\discriminative\\fuzzySimilarity\\evaluationTitleText.txt");
            string strEvaluationText = srEvaluationText.ReadLine();


            string[] aryDocBelonbgsToClusterTitle = new string[docNoEvaluation];


            for (int i = 0; i < docNoEvaluation; i++)
            {
                strEvaluationText = strEvaluationText.Replace("\t", " ");
                strEvaluationText = strEvaluationText.Replace("\n", " ");
                strEvaluationText = strEvaluationText.Replace("\r", " ");
                strEvaluationText = strEvaluationText.Replace("\b", " ");
                strEvaluationText = strEvaluationText.Replace("\f", " ");
                strEvaluationText = strEvaluationText.Replace("\n", " ");
                strEvaluationText = strEvaluationText.Replace("\v", " ");
                strEvaluationText = strEvaluationText.Replace("\a", " ");
                strEvaluationText = strEvaluationText.Replace("\0", " ");
                strEvaluationText = strEvaluationText.Replace("\\", " ");
                strEvaluationText = strEvaluationText.Replace("\'", " ");
                strEvaluationText = strEvaluationText.Replace("\"", " ");

                strEvaluationText = string.Join(" ", strEvaluationText.Split(' ').Distinct());

                string[] aryEvaluationText = strEvaluationText.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);



                aryDocBelonbgsToClusterTitle[i] = aryEvaluationText[0];


                strEvaluationText = srEvaluationText.ReadLine();
            }
            srEvaluationText.Close();

            //========== CREATING EVALUATION CLUSTER-ID === (OPTIONAL 3) === (IMPORTANT just in a special case no need to input else should be comment this part)

            //for (int i = 0; i < docNoEvaluation; i++)
            //{
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("acq", "1");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("crude", "2");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("earn", "3");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("grain", "4");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("interest", "5");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("money-fx", "6");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("ship", "7");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("trade", "8");
            //}

            //for (int i = 0; i < docNoEvaluation; i++)
            //{
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("اجتماعی", "1");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("اقتصادی", "2");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("دینی", "3");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("سیاسی", "4");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("فناوری", "5");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("فرهنگی", "6");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("ورزشی", "7");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("صنعت", "8");
            //}

            //for (int i = 0; i < docNoEvaluation; i++)
            //{
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("آموزشي", "1");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("خودرو", "2");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("قهرماني", "3");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("حوادث", "4");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("بورس", "5");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("رسانه", "6");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("تاريخ", "7");
            //    aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("علمي", "8");
            //}

            for (int i = 0; i < docNoEvaluation; i++)
            {
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("alt.atheism", "1");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("comp.graphics", "2");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("comp.os.ms-windows.misc", "3");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("comp.sys.ibm.pc.hardware", "4");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("comp.sys.mac.hardware", "5");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("comp.windows.x", "6");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("misc.forsale", "7");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("rec.autos", "8");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("rec.motorcycles", "9");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("rec.sport.baseball", "10");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("rec.sport.hockey", "11");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("sci.crypt", "12");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("sci.electronics", "13");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("sci.med", "14");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("sci.space", "15");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("soc.religion.christian", "16");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("talk.politics.guns", "17");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("talk.politics.mideast", "18");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("talk.politics.misc", "19");
                aryDocBelonbgsToClusterTitle[i] = aryDocBelonbgsToClusterTitle[i].Replace("talk.religion.misc", "20");
            }


            int[] countOfClustersDocument = new int[clusterNo];
            for (int noOfClusters = 0; noOfClusters < clusterNo; noOfClusters++)
            {
                int clusterCounter = 0;
                for (int noOfDocs = 0; noOfDocs < docNoEvaluation; noOfDocs++)
                {
                    if (int.Parse(aryDocBelonbgsToClusterTitle[noOfDocs]) - 1 == noOfClusters)
                    {
                        clusterCounter++;
                    }
                }

                countOfClustersDocument[noOfClusters] = clusterCounter;
            }

            //++++++++++

            StreamWriter swAryDocBelonbgsToClusterTitle = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\evaluationClusterID.txt");
            for (int i = 0; i < docNoEvaluation; i++)
            {
                swAryDocBelonbgsToClusterTitle.WriteLine(aryDocBelonbgsToClusterTitle[i]);
            }
            swAryDocBelonbgsToClusterTitle.Close();

            //========== CREATING NO TITLE = evaluationText (also can to be input) 

            StreamReader srOmitTitleEvaluationText = new StreamReader("d:\\discriminative\\fuzzySimilarity\\evaluationTitleText.txt");

            string[] omitted = new string[docNoEvaluation];

            string strOmitTitleEvaluationText = srOmitTitleEvaluationText.ReadLine();
            int o = 0;
            for (int i = 0; i < docNoEvaluation; i++)
            {
                string lstToStr = "";
                strOmitTitleEvaluationText = strOmitTitleEvaluationText.Replace("\t", " ");
                strOmitTitleEvaluationText = strOmitTitleEvaluationText.Replace("\n", " ");
                strOmitTitleEvaluationText = strOmitTitleEvaluationText.Replace("\r", " ");
                strOmitTitleEvaluationText = strOmitTitleEvaluationText.Replace("\b", " ");
                strOmitTitleEvaluationText = strOmitTitleEvaluationText.Replace("\f", " ");
                strOmitTitleEvaluationText = strOmitTitleEvaluationText.Replace("\n", " ");
                strOmitTitleEvaluationText = strOmitTitleEvaluationText.Replace("\v", " ");
                strOmitTitleEvaluationText = strOmitTitleEvaluationText.Replace("\a", " ");
                strOmitTitleEvaluationText = strOmitTitleEvaluationText.Replace("\0", " ");
                strOmitTitleEvaluationText = strOmitTitleEvaluationText.Replace("\\", " ");
                strOmitTitleEvaluationText = strOmitTitleEvaluationText.Replace("\'", " ");
                strOmitTitleEvaluationText = strOmitTitleEvaluationText.Replace("\"", " ");

                string[] aryOmitTitleEvaluationText = strOmitTitleEvaluationText.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                List<string> lstOmitTitleEvaluationText = new List<string>(aryOmitTitleEvaluationText);

                lstOmitTitleEvaluationText.RemoveAt(0);

                for (int b = 0; b < lstOmitTitleEvaluationText.Count; b++)
                {
                    lstToStr = lstToStr + lstOmitTitleEvaluationText[b] + " ";
                }

                omitted[o] = lstToStr;
                o++;

                strOmitTitleEvaluationText = srOmitTitleEvaluationText.ReadLine();
            }
            srOmitTitleEvaluationText.Close();

            //++++++++++

            StreamWriter swOmitTitleEvaluationText = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\evaluationText.txt");
            for (int i = 0; i < docNoEvaluation; i++)
            {
                swOmitTitleEvaluationText.WriteLine(omitted[i]);
            }
            swOmitTitleEvaluationText.Close();

            //========== EVALUATION Text DocNo === (IMPORTANT = if (OPTIONAL 2) commented, here lentgh-1 should converted to lentgh )

            //StreamReader srDocNoEvaluation = new StreamReader("d:\\discriminative\\fuzzySimilarity\\evaluationText.txt");
            //string strDocNoEvaluation = srDocNoEvaluation.ReadToEnd();
            //string[] aryDocNoEvaluation = strDocNoEvaluation.Split('\n');
            //docNoEvaluation = aryDocNoEvaluation.Length - 1;  // previous write one extra \n Added to last line


            //========== SIMILARITY FOR EVALUATION TEXTS 

            StreamReader srEvaluationTextSpecificTermList = new StreamReader("d:\\discriminative\\fuzzySimilarity\\evaluationText.txt");
            string strEvaluationTextSpecificTermList = srEvaluationTextSpecificTermList.ReadLine();

            StreamReader srEvaluationTextAllTermList = new StreamReader("d:\\discriminative\\fuzzySimilarity\\evaluationText.txt");
            string strEvaluationTextAllTermList = srEvaluationTextAllTermList.ReadLine();

            // // // // // Reading matrixTestDocTFIDF

            //StreamReader srTFIDF = new StreamReader("d:\\discriminative\\matrixTestDocTFIDF.txt");
            //float[,] matrixTFIDF = new float[docNoEvaluation, termNo];
            //string[] arySampleTFIDF;
            //for (int countermatrixTestDocTFIDF = 0; countermatrixTestDocTFIDF < docNoEvaluation; countermatrixTestDocTFIDF++)
            //{
            //    string strTFIDF = srTFIDF.ReadLine();
            //    arySampleTFIDF = strTFIDF.Split(' ');
            //    for (int vvw = 0; vvw < termNo; vvw++)
            //    {
            //        matrixTFIDF[countermatrixTestDocTFIDF, vvw] = Convert.ToSingle(arySampleTFIDF[vvw]);
            //    }
            //}
            //srTFIDF.Close();

            // // // // //


            // // // // // Adding some Wheights instead of M(dt)

            //StreamReader srAryTestDocTFIDFModified = new StreamReader("d:\\discriminative\\matrixTestDocTFIDFModified.txt");


            // // // // //

            StreamWriter swEvaluationTextSpecificTermList = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\evaluationSimilarities.txt");
            StreamWriter swDocSimilarToCluster = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\testDocsSimilarToCluster.txt");
            float[] aryMultipleCategoriesDivideBySingleCategories = new float[docNoEvaluation];
            for (int i = 0; i < docNoEvaluation; i++)
            {

                strEvaluationTextSpecificTermList = strEvaluationTextSpecificTermList.Replace("\t", " ");
                strEvaluationTextSpecificTermList = strEvaluationTextSpecificTermList.Replace("\n", " ");
                strEvaluationTextSpecificTermList = strEvaluationTextSpecificTermList.Replace("\r", " ");
                strEvaluationTextSpecificTermList = strEvaluationTextSpecificTermList.Replace("\b", " ");
                strEvaluationTextSpecificTermList = strEvaluationTextSpecificTermList.Replace("\f", " ");
                strEvaluationTextSpecificTermList = strEvaluationTextSpecificTermList.Replace("\n", " ");
                strEvaluationTextSpecificTermList = strEvaluationTextSpecificTermList.Replace("\v", " ");
                strEvaluationTextSpecificTermList = strEvaluationTextSpecificTermList.Replace("\a", " ");
                strEvaluationTextSpecificTermList = strEvaluationTextSpecificTermList.Replace("\0", " ");
                strEvaluationTextSpecificTermList = strEvaluationTextSpecificTermList.Replace("\\", " ");
                strEvaluationTextSpecificTermList = strEvaluationTextSpecificTermList.Replace("\'", " ");
                strEvaluationTextSpecificTermList = strEvaluationTextSpecificTermList.Replace("\"", " ");

                strEvaluationTextSpecificTermList = string.Join(" ", strEvaluationTextSpecificTermList.Split(' ').Distinct());

                string[] aryEvaluationTextSpecificTermList = strEvaluationTextSpecificTermList.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);




                strEvaluationTextAllTermList = strEvaluationTextAllTermList.Replace("\t", " ");
                strEvaluationTextAllTermList = strEvaluationTextAllTermList.Replace("\n", " ");
                strEvaluationTextAllTermList = strEvaluationTextAllTermList.Replace("\r", " ");
                strEvaluationTextAllTermList = strEvaluationTextAllTermList.Replace("\b", " ");
                strEvaluationTextAllTermList = strEvaluationTextAllTermList.Replace("\f", " ");
                strEvaluationTextAllTermList = strEvaluationTextAllTermList.Replace("\n", " ");
                strEvaluationTextAllTermList = strEvaluationTextAllTermList.Replace("\v", " ");
                strEvaluationTextAllTermList = strEvaluationTextAllTermList.Replace("\a", " ");
                strEvaluationTextAllTermList = strEvaluationTextAllTermList.Replace("\0", " ");
                strEvaluationTextAllTermList = strEvaluationTextAllTermList.Replace("\\", " ");
                strEvaluationTextAllTermList = strEvaluationTextAllTermList.Replace("\'", " ");
                strEvaluationTextAllTermList = strEvaluationTextAllTermList.Replace("\"", " ");

                string[] aryEvaluationTextAllTermList = strEvaluationTextAllTermList.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                //    //========== GETTING EVALUATION DOC VECTORS 

                //    //////////////////////////////////float[] aryTermCounter = new float[aryEvaluationTextSpecificTermList.Length];
                //    //////////////////////////////////for (int z = 0; z < aryEvaluationTextSpecificTermList.Length; z++)
                //    //////////////////////////////////{
                //    //////////////////////////////////    float evaluationStrCounter = 0;
                //    //////////////////////////////////    for (int j = 0; j < aryEvaluationTextAllTermList.Length; j++)
                //    //////////////////////////////////    {
                //    //////////////////////////////////        if (aryEvaluationTextSpecificTermList[z] == aryEvaluationTextAllTermList[j])
                //    //////////////////////////////////        {
                //    //////////////////////////////////            evaluationStrCounter = evaluationStrCounter + 1;
                //    //////////////////////////////////        }
                //    //////////////////////////////////    }
                //    //////////////////////////////////    aryTermCounter[z] = evaluationStrCounter;
                //    //////////////////////////////////}



                float[] evaluationTestVector = new float[termNo];
                for (int zz = 0; zz < termNo; zz++)
                {
                    float evaluationStrCounter = 0;
                    for (int j = 0; j < aryEvaluationTextAllTermList.Length; j++)
                    {
                        if (lst[zz] == aryEvaluationTextAllTermList[j])
                        {
                            evaluationStrCounter = evaluationStrCounter + 1;

                        }
                    }
                    evaluationTestVector[zz] = evaluationStrCounter;
                }


                //========== Md(t) EVALUATION 

                // // // // // Adding some Wheights instead of M(dt)

                //string strSample = srAryTestDocTFIDFModified.ReadLine();
                //string[] arySample = strSample.Split(' ');

                //float[] MooDTEvaluation = new float[termNo];

                //for (int tiker = 0; tiker < termNo; tiker++)
                //{
                //    MooDTEvaluation[tiker] = Convert.ToSingle(arySample[tiker]);
                //}

                // // // // // 

                //---------- Normalization = Paper Method = W / W max 

                float MooDTDenominatorEvaluation = evaluationTestVector.Max();
                float[] MooDTEvaluation2 = new float[termNo];

                for (int j = 0; j < termNo; j++)
                {
                    if (evaluationTestVector[j] != 0)
                    {
                        MooDTEvaluation2[j] = evaluationTestVector[j] / MooDTDenominatorEvaluation;
                    }
                }

                //---------- Binary Method

                int[] MooDTEvaluation3 = new int[termNo];

                for (int j = 0; j < termNo; j++)
                {
                    if (evaluationTestVector[j] != 0)
                    {
                        MooDTEvaluation3[j] = 1;
                    }
                }

                //---------- Membership Method W / All Test W

                float sumOfCellsOfEvaluationTestVector = 0;
                for (int h = 0; h < termNo; h++)
                {
                    sumOfCellsOfEvaluationTestVector += evaluationTestVector[h];
                }
                float MooDTDenominatorEvaluation2 = sumOfCellsOfEvaluationTestVector;

                float[] MooDTEvaluation4 = new float[termNo];

                for (int j = 0; j < termNo; j++)
                {
                    if (evaluationTestVector[j] != 0)
                    {
                        MooDTEvaluation4[j] = evaluationTestVector[j] / MooDTDenominatorEvaluation2;
                    }
                }

                //---------- TF IDF

                //float[] MooDTEvaluation5 = new float[termNo];

                //for (int j = 0; j < termNo; j++)
                //{
                //    int nm = 0;
                //    for (int vvx = 0; vvx < docNoEvaluation; vvx++)
                //    {
                //        if (matrixTFIDF[i, j] != 0)
                //        {
                //            nm++;
                //        }
                //    }


                //    if (evaluationTestVector[j] != 0)
                //    {
                //        MooDTEvaluation5[j] = evaluationTestVector[j] * Convert.ToSingle(Math.Log10(docNoEvaluation / (nm + 0.01)));
                //    }
                //}

                //---------- TF-IDF (Normalized = DFS Paper Mode )

                //float[] MooDTEvaluation6 = new float[termNo];

                //for (int j = 0; j < termNo; j++)
                //{
                //    MooDTEvaluation6[j] = matrixTFIDF[i, j];
                //}

                //++++++++++

                float evaluationDownSim, evaluationUpSim;
                float[] evaluationAryUpSim = new float[aryEvaluationTextSpecificTermList.Length];
                float[] evaluationAryDownSim = new float[aryEvaluationTextSpecificTermList.Length];
                float[,] evaluationMatrixUpSim = new float[aryEvaluationTextSpecificTermList.Length, clusterNo];
                float[,] evaluationMatrixDownSim = new float[aryEvaluationTextSpecificTermList.Length, clusterNo];

                for (int c = 0; c < clusterNo; c++)
                {
                    for (int k = 0; k < aryEvaluationTextSpecificTermList.Length; k++)
                    {
                        for (int j = 0; j < termNo; j++)
                        {
                            if (aryEvaluationTextSpecificTermList[k] == lst[j])
                            {











                                // // // //
                                // 4.DFS + Fuzzy Similarity
                                // 5.DFS + Hamakher  
                                // 7.DFS SIM + Fuzzy Similarity
                                // 8.DFS SIM + Hamakher

                                float[] up = new float[100];
                                //up[0] = matrixProbability[j, c] + (MooDTEvaluation[j] * MooDTEvaluation2[j]); /*Best 20NewsGRopu*/
                                //up[1] = matrixProbability[j, c] * Math.Abs(MooDTEvaluation[j] - MooDTEvaluation2[j]); /*reuters*/
                                //up[2] = ((matrixProbability[j, c] + (MooDTEvaluation[j] * MooDTEvaluation2[j])) + (matrixProbability[j, c] * (MooDTEvaluation[j] + MooDTEvaluation2[j]))) - ((matrixProbability[j, c] + (MooDTEvaluation[j] * MooDTEvaluation2[j])) * (matrixProbability[j, c] * (MooDTEvaluation[j] + MooDTEvaluation2[j])));
                                //up[3] = (matrixProbability[j, c] * MooDTEvaluation2[j]) / ((matrixProbability[j, c] + MooDTEvaluation2[j]) - (matrixProbability[j, c] * MooDTEvaluation2[j])); /* Persian */
                                up[4] /*down*/= (matrixProbability[j, c] * MooDTEvaluation2[j]); /*reuters*/
                                //up[5] /*down*/= (matrixProbability[j, c] * MooDTEvaluation2[j]) / (matrixProbability[j, c] + MooDTEvaluation2[j]) - (matrixProbability[j, c] * MooDTEvaluation2[j]);
                                //up[6] = (matrixProbability[j, c] + MooDTEvaluation[j]) - (matrixProbability[j, c] * MooDTEvaluation[j]);
                                //up[7] /*down*/ = (matrixProbability[j, c] * MooDTEvaluation[j]);
                                //up[8] /*down*/= (matrixProbability[j, c] * MooDTEvaluation[j]) / (matrixProbability[j, c] + MooDTEvaluation[j]) - (matrixProbability[j, c] * MooDTEvaluation[j]);
                                //up[9] = (matrixProbability[j, c] + MooDTEvaluation[j]) - (matrixProbability[j, c] * MooDTEvaluation[j]);
                                //up[10] = (matrixProbability[j, c] + MooDTEvaluation2[j]) - (matrixProbability[j, c] * MooDTEvaluation2[j]);
                                //up[11] = (matrixProbability[j, c] + MooDTEvaluation[j]) - (matrixProbability[j, c] * MooDTEvaluation2[j]);
                                //up[12] = (matrixProbability[j, c] + MooDTEvaluation2[j]) - (matrixProbability[j, c] * MooDTEvaluation[j]);
                                //up[13] = Math.Abs((matrixProbability[j, c] * MooDTEvaluation2[j]) - (matrixProbability[j, c] * MooDTEvaluation[j])); /* reuters*/
                                //up[14] = ((matrixProbability[j, c] * MooDTEvaluation2[j]) + (matrixProbability[j, c] * MooDTEvaluation[j])); /*reuters*/  /* Persian*/  /* 20NewsGroup Best 5000*/  /* 20NewsGroup */
                                //up[15]/*down*/ = (matrixProbability[j, c] * MooDTEvaluation2[j]);
                                //up[16]/*down*/= (matrixProbability[j, c] + MooDTEvaluation2[j]);
                                //up[17] = matrixProbability[j, c] + MooDTEvaluation[j] + MooDTEvaluation2[j];
                                //up[18] /*down*/= (matrixProbability[j, c] * MooDTEvaluation2[j]) + (matrixProbability[j, c] * MooDTEvaluation[j]);  /* reuters*/  /*20news 5000*/
                                //up[19] = ((matrixProbability[j, c] * MooDTEvaluation2[j]) + (matrixProbability[j, c] * MooDTEvaluation[j])) + Math.Abs(matrixProbability[j, c] * MooDTEvaluation2[j] * MooDTEvaluation[j]);  /* 20 NewsGroup*/ /* Best Persian */
                                //up[20] /*down*/= ((matrixProbability[j, c] * MooDTEvaluation2[j]) + (matrixProbability[j, c] * MooDTEvaluation[j])) + Math.Abs(matrixProbability[j, c] * MooDTEvaluation2[j] * MooDTEvaluation[j]);  /* Best reuters*/
                                //up[21] /*down*/= Math.Abs(matrixProbability[j, c] - MooDTEvaluation[j]) * MooDTEvaluation2[j]; /*reuters*/
                                //up[22] = ((matrixProbability[j, c] * MooDTEvaluation2[j]) + (matrixProbability[j, c] * MooDTEvaluation[j])) + Math.Abs(matrixProbability[j, c] * Math.Abs(MooDTEvaluation2[j] - MooDTEvaluation[j]));
                                //up[23] = ((matrixProbability[j, c] * MooDTEvaluation2[j]) + (matrixProbability[j, c] * MooDTEvaluation[j])) + Math.Abs(matrixProbability[j, c] + Math.Abs(MooDTEvaluation2[j] * MooDTEvaluation[j]));

                                evaluationUpSim = up[4];

                                //---

                                float[] down = new float[100];
                                down[0] = 1;
                                down[4] = (matrixProbability[j, c] + MooDTEvaluation2[j]) - (matrixProbability[j, c] * MooDTEvaluation2[j]);
                                //down[5] = ((matrixProbability[j, c] + MooDTEvaluation2[j]) - (2 * matrixProbability[j, c] * MooDTEvaluation2[j])) / ((1 - (matrixProbability[j, c] * MooDTEvaluation2[j])));
                                //down[7] = ((matrixProbability[j, c] + MooDTEvaluation[j]) - (matrixProbability[j, c] * MooDTEvaluation[j]));
                                //down[8] = ((matrixProbability[j, c] + MooDTEvaluation[j]) - (2 * matrixProbability[j, c] * MooDTEvaluation[j])) / ((1 - (matrixProbability[j, c] * MooDTEvaluation[j])));
                                //down[15] = matrixProbability[j, c] * MooDTEvaluation[j];
                                //down[16] = matrixProbability[j, c] + MooDTEvaluation[j];
                                //down[18] = ((matrixProbability[j, c] + MooDTEvaluation2[j]) - (matrixProbability[j, c] * MooDTEvaluation2[j])) + ((matrixProbability[j, c] + MooDTEvaluation[j]) - (matrixProbability[j, c] * MooDTEvaluation[j]));
                                //down[20] = ((matrixProbability[j, c] + MooDTEvaluation2[j]) - (matrixProbability[j, c] * MooDTEvaluation2[j])) + ((matrixProbability[j, c] + MooDTEvaluation[j]) - (matrixProbability[j, c] * MooDTEvaluation[j])) + Math.Abs((matrixProbability[j, c] + MooDTEvaluation2[j] + MooDTEvaluation[j]) - (matrixProbability[j, c] * MooDTEvaluation2[j] * MooDTEvaluation[j]));
                                //down[21] = (Math.Abs(matrixProbability[j, c] - MooDTEvaluation[j]) + MooDTEvaluation2[j]) - (Math.Abs(matrixProbability[j, c] - MooDTEvaluation[j]) * MooDTEvaluation2[j]);

                                evaluationDownSim = down[4];










                                // // // //
                                evaluationAryUpSim[k] = evaluationUpSim;
                                evaluationAryDownSim[k] = evaluationDownSim;
                                evaluationMatrixUpSim[k, c] = evaluationUpSim;
                                evaluationMatrixDownSim[k, c] = evaluationDownSim;
                            }
                        }
                    }
                }




                float[] evaluationAryUpSummations = new float[clusterNo];
                float[] evaluationAryDownSummations = new float[clusterNo];
                float evaluationUpSummations, evaluationDownSummations;
                for (int c = 0; c < clusterNo; c++)
                {
                    evaluationUpSummations = 0;
                    for (int d = 0; d < aryEvaluationTextSpecificTermList.Length; d++)
                    {
                        evaluationUpSummations = evaluationMatrixUpSim[d, c] + evaluationUpSummations;
                    }
                    evaluationAryUpSummations[c] = evaluationUpSummations;
                }


                for (int c = 0; c < clusterNo; c++)
                {
                    evaluationDownSummations = 0;
                    for (int e = 0; e < aryEvaluationTextSpecificTermList.Length; e++)
                    {
                        evaluationDownSummations = evaluationMatrixDownSim[e, c] + evaluationDownSummations;
                    }
                    evaluationAryDownSummations[c] = evaluationDownSummations;
                }


                float[] evaluationArySimilarity = new float[clusterNo];
                for (int c = 0; c < clusterNo; c++)
                {
                    evaluationArySimilarity[c] = evaluationAryUpSummations[c] / evaluationAryDownSummations[c];
                }

                float summerOfDocumentSimilarToCluster = 0;
                foreach (float docSimilarToCluster in evaluationArySimilarity)
                {
                    swDocSimilarToCluster.Write(docSimilarToCluster + " ");
                    summerOfDocumentSimilarToCluster += docSimilarToCluster;
                }
                swDocSimilarToCluster.WriteLine();

                float evaluationMax = evaluationArySimilarity.Max();
                float summerOfDocumentSimilarToClusterDevideByEvaluationMax = summerOfDocumentSimilarToCluster / evaluationMax;
                aryMultipleCategoriesDivideBySingleCategories[i] = summerOfDocumentSimilarToClusterDevideByEvaluationMax;

                int evaluationMaxCluster = 0;
                for (int c = 0; c < clusterNo; c++)
                {
                    if (evaluationArySimilarity[c] == evaluationMax)
                    {
                        evaluationMaxCluster = c + 1;
                    }
                }

                swEvaluationTextSpecificTermList.WriteLine(evaluationMaxCluster);
                //swEvaluationTextSpecificTermList.WriteLine("{0}-{1}", evaluationMaxCluster, evaluationMax);
                // Should omit Part CREATING CONFUSION MATRIX to the END see upper description.
                strEvaluationTextAllTermList = srEvaluationTextAllTermList.ReadLine();
                strEvaluationTextSpecificTermList = srEvaluationTextSpecificTermList.ReadLine();

            }

            // // // // // Adding some Wheights instead of M(dt)
            //srAryTestDocTFIDFModified.Close();
            // // // // //

            swDocSimilarToCluster.Close();
            srEvaluationTextAllTermList.Close();
            srEvaluationTextSpecificTermList.Close();
            swEvaluationTextSpecificTermList.Close();

            //--- Next Application



            for (int noOfLabels = 0; noOfLabels < docNoEvaluation; noOfLabels++)
            {
                aryDocBelonbgsToClusterTitle[noOfLabels] = aryDocBelonbgsToClusterTitle[noOfLabels].Replace("1", "acq");
                aryDocBelonbgsToClusterTitle[noOfLabels] = aryDocBelonbgsToClusterTitle[noOfLabels].Replace("2", "crude");
                aryDocBelonbgsToClusterTitle[noOfLabels] = aryDocBelonbgsToClusterTitle[noOfLabels].Replace("3", "earn");
                aryDocBelonbgsToClusterTitle[noOfLabels] = aryDocBelonbgsToClusterTitle[noOfLabels].Replace("4", "grain");
                aryDocBelonbgsToClusterTitle[noOfLabels] = aryDocBelonbgsToClusterTitle[noOfLabels].Replace("5", "interest");
                aryDocBelonbgsToClusterTitle[noOfLabels] = aryDocBelonbgsToClusterTitle[noOfLabels].Replace("6", "money-fx");
                aryDocBelonbgsToClusterTitle[noOfLabels] = aryDocBelonbgsToClusterTitle[noOfLabels].Replace("7", "ship");
                aryDocBelonbgsToClusterTitle[noOfLabels] = aryDocBelonbgsToClusterTitle[noOfLabels].Replace("8", "trade");
            }

            int[] indexes = new int[aryMultipleCategoriesDivideBySingleCategories.Length];
            for (int maxOfNumbers = 0; maxOfNumbers < aryMultipleCategoriesDivideBySingleCategories.Length; maxOfNumbers++)
            {
                float maxOf = aryMultipleCategoriesDivideBySingleCategories.Max();
                indexes[maxOfNumbers] = aryMultipleCategoriesDivideBySingleCategories.ToList().IndexOf(maxOf);
                aryMultipleCategoriesDivideBySingleCategories[aryMultipleCategoriesDivideBySingleCategories.ToList().IndexOf(maxOf)] = -1;
            }

            StreamWriter swIndexes = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\IndexesAll.txt");
            foreach (int sample in indexes)
                swIndexes.WriteLine(sample);
            swIndexes.Close();


            int[] aryIndexMultiCategories = new int[Alpha];
            int[] aryIndexSingleCategories = new int[indexes.Length - Alpha];
            string[] aryMultiCategories = new string[Alpha];
            string[] arySingleCategories = new string[indexes.Length - Alpha];

            for (int idxMulti = 0; idxMulti < Alpha; idxMulti++)
            {
                aryIndexMultiCategories[idxMulti] = indexes[idxMulti];
            }
            int idxArray = 0;
            for (int idxSingle = Alpha; idxSingle < indexes.Length; idxSingle++)
            {
                aryIndexSingleCategories[idxArray] = indexes[idxSingle];
                idxArray++;
            }


            for (int noOfMultiCategories = 0; noOfMultiCategories < Alpha; noOfMultiCategories++)
            {
                int NUMBER = indexes[noOfMultiCategories];
                aryMultiCategories[noOfMultiCategories] = aryDocBelonbgsToClusterTitle[NUMBER] + "  " + omitted[NUMBER];
            }

            int arySingleCategoriesIndex = 0;
            for (int noOfSingleCategories = Alpha; noOfSingleCategories < indexes.Length; noOfSingleCategories++)
            {
                int NUMBER2 = indexes[noOfSingleCategories];
                arySingleCategories[arySingleCategoriesIndex] = aryDocBelonbgsToClusterTitle[NUMBER2] + "    " + omitted[NUMBER2];
                arySingleCategoriesIndex++;
            }

            //*** aryIndex01

            int[] aryIndex01 = new int[indexes.Length];
            for (int noIdx01 = 0; noIdx01 < indexes.Length; noIdx01++)
            {
                for (int noIdxAryMultiCategories = 0; noIdxAryMultiCategories < Alpha; noIdxAryMultiCategories++)
                {
                    int substitute = indexes[noIdx01];

                    if (indexes[noIdx01] == aryIndexMultiCategories[noIdxAryMultiCategories])
                    {
                        aryIndex01[substitute] = 1;
                    }
                }
            }

            StreamReader sr01 = new StreamReader("d:\\discriminative\\fuzzySimilarity\\01.txt");
            int[] ary01 = new int[indexes.Length];
            for (int no01 = 0; no01 < indexes.Length; no01++)
            {
                ary01[no01] = int.Parse(sr01.ReadLine());
            }
            sr01.Close();


            float TP1 = 0;
            float FP1 = 0;
            float TN1 = 0;
            float FN1 = 0;
            float PRECISION1;
            float RECALL1;
            float FMEASURE1;

            for (int member = 0; member < aryIndex01.Length; member++)
            {
                if (aryIndex01[member] == 1 && ary01[member] == 1)
                {
                    TP1++;
                }

                if (aryIndex01[member] == 0 && ary01[member] == 0)
                {
                    TN1++;
                }

                if (aryIndex01[member] == 0 && ary01[member] == 1)
                {
                    FN1++;
                }

                if (aryIndex01[member] == 1 && ary01[member] == 0)
                {
                    FP1++;
                }
            }

            PRECISION1 = TP1 / (TP1 + FP1);
            RECALL1 = TP1 / (TP1 + FN1);
            FMEASURE1 = (2 * PRECISION1 * RECALL1) / (PRECISION1 + RECALL1);

            float TP0 = 0;
            float FP0 = 0;
            float TN0 = 0;
            float FN0 = 0;
            float PRECISION0;
            float RECALL0;
            float FMEASURE0;

            for (int member2 = 0; member2 < aryIndex01.Length; member2++)
            {
                if (aryIndex01[member2] == 1 && ary01[member2] == 1)
                {
                    TN0++;
                }

                if (aryIndex01[member2] == 0 && ary01[member2] == 0)
                {
                    TP0++;
                }

                if (aryIndex01[member2] == 0 && ary01[member2] == 1)
                {
                    FP0++;
                }

                if (aryIndex01[member2] == 1 && ary01[member2] == 0)
                {
                    FN0++;
                }
            }
            PRECISION0 = TP0 / (TP0 + FP0);
            RECALL0 = TP0 / (TP0 + FN0);
            FMEASURE0 = (2 * PRECISION0 * RECALL0) / (PRECISION0 + RECALL0);

            //***

            StreamWriter swAryMultiCategories = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\multiCategories.txt");
            for (int one = 0; one < aryMultiCategories.Length; one++)
            {
                if (one != aryMultiCategories.Length - 1)
                {
                    swAryMultiCategories.WriteLine(aryMultiCategories[one]);
                }
                else
                {
                    swAryMultiCategories.Write(aryMultiCategories[one]);
                }
            }
            swAryMultiCategories.Close();

            StreamWriter swArySingleCategories = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\singleCategories.txt");
            for (int two = 0; two < arySingleCategories.Length; two++)
            {
                if (two != arySingleCategories.Length - 1)
                {
                    swArySingleCategories.WriteLine(arySingleCategories[two]);
                }
                else
                {
                    swArySingleCategories.Write(arySingleCategories[two]);
                }
            }
            swArySingleCategories.Close();


            //---


            //***************************************************************************************************************************************




            //========== COMPUTING THE RECALL & PRECISION & F-MEASURE 

            StreamReader srEvaluationSimilarities = new StreamReader("d:\\discriminative\\fuzzySimilarity\\evaluationSimilarities.txt");
            int[] aryEvaluationSimilarities = new int[docNoEvaluation];
            for (int i = 0; i < docNoEvaluation; i++)
            {
                aryEvaluationSimilarities[i] = (int.Parse(srEvaluationSimilarities.ReadLine())) - 1;
            }
            srEvaluationSimilarities.Close();


            StreamReader srEvaluationClusterID = new StreamReader("d:\\discriminative\\fuzzySimilarity\\evaluationClusterID.txt");
            int[] aryEvaluationClusterID = new int[docNoEvaluation];
            for (int i = 0; i < docNoEvaluation; i++)
            {
                aryEvaluationClusterID[i] = (int.Parse(srEvaluationClusterID.ReadLine())) - 1;
            }
            srEvaluationClusterID.Close();

            //++++++++++

            float[] truePositive = new float[clusterNo];
            float[] falseNegative = new float[clusterNo];

            for (int c = 0; c < clusterNo; c++)
            {
                int truePositiveCounter = 0;
                int falseNegativeCounter = 0;

                for (int i = 0; i < docNoEvaluation; i++)
                {
                    if (c == aryEvaluationClusterID[i])
                    {
                        if (aryEvaluationClusterID[i] == aryEvaluationSimilarities[i])
                        {
                            truePositiveCounter = truePositiveCounter + 1;
                        }
                        else
                        {
                            falseNegativeCounter = falseNegativeCounter + 1;
                        }
                    }
                }
                truePositive[c] = truePositiveCounter;
                falseNegative[c] = falseNegativeCounter;
            }



            float[] falsePositive = new float[clusterNo];

            for (int c = 0; c < clusterNo; c++)
            {
                int falsePositiveCounter = 0;
                for (int i = 0; i < docNoEvaluation; i++)
                {
                    if (c == aryEvaluationSimilarities[i])
                    {
                        falsePositiveCounter = falsePositiveCounter + 1;
                    }
                }
                falsePositive[c] = falsePositiveCounter;
            }

            for (int c = 0; c < clusterNo; c++)
            {
                falsePositive[c] = falsePositive[c] - truePositive[c];
            }





            float sumTruePositive = 0;
            for (int c = 0; c < clusterNo; c++)
            {
                sumTruePositive = truePositive[c] + sumTruePositive;
            }


            float sumFalsePositive = 0;
            for (int c = 0; c < clusterNo; c++)
            {
                sumFalsePositive = falsePositive[c] + sumFalsePositive;
            }


            float sumFalseNegative = 0;
            for (int c = 0; c < clusterNo; c++)
            {
                sumFalseNegative = falseNegative[c] + sumFalseNegative;
            }

            //--- Micro

            float precision = sumTruePositive / (sumTruePositive + sumFalsePositive);

            float recall = sumTruePositive / (sumTruePositive + sumFalseNegative);

            float FMeasure = (2 * precision * recall) / (precision + recall);

            //--- 

            //++++++++++

            StreamWriter swMatrixConfusion = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\matrixConfusion.txt");

            for (int c = 0; c < clusterNo; c++)
            {
                swMatrixConfusion.Write("{0} {1} {2}", truePositive[c], falsePositive[c], falseNegative[c]);
                swMatrixConfusion.WriteLine();
            }
            swMatrixConfusion.Close();

            //========== EVALUATION CLUSTER BY CLUSTER 

            float[] aryPrecision = new float[clusterNo];
            float[] aryRecall = new float[clusterNo];
            float[] aryFMeasure = new float[clusterNo];

            for (int c = 0; c < clusterNo; c++)
            {
                if (truePositive[c] != 0)
                {
                    aryPrecision[c] = truePositive[c] / (truePositive[c] + falsePositive[c]);
                    aryRecall[c] = truePositive[c] / (truePositive[c] + falseNegative[c]);
                    aryFMeasure[c] = (2 * ((aryPrecision[c]) * (aryRecall[c]))) / (aryPrecision[c] + aryRecall[c]);
                }
                else
                {
                    aryPrecision[c] = 0;
                    aryRecall[c] = 0;
                    aryFMeasure[c] = 0;
                }
            }

              //--- F-Macro

                float FMacro = 0;
                for (int c = 0; c < clusterNo; c++)
                {
                    FMacro += aryFMeasure[c];
                }
                FMacro = FMacro / clusterNo;

              //---
            

            float summerOfPrecisionOfClustersForAverage = 0;
            float summerOfRecallOfClustersForAverage = 0;
            float summerOfFMeasureOfClustersForAverage = 0;
            for (int c = 0; c < clusterNo; c++)
            {
                summerOfPrecisionOfClustersForAverage += aryPrecision[c];
                summerOfRecallOfClustersForAverage += aryRecall[c];
                summerOfFMeasureOfClustersForAverage += aryFMeasure[c];
            }
            summerOfPrecisionOfClustersForAverage /= clusterNo;
            summerOfRecallOfClustersForAverage /= clusterNo;
            summerOfFMeasureOfClustersForAverage /= clusterNo;



            float summerOfPrecisionOfClustersForWeightedAverage = 0;
            float summerOfRecallOfClustersForWeightedAverage = 0;
            float summerOfFMeasureOfClustersForWeightedAverage = 0;
            for (int c = 0; c < clusterNo; c++)
            {
                summerOfPrecisionOfClustersForWeightedAverage += aryPrecision[c] * countOfClustersDocument[c];
                summerOfRecallOfClustersForWeightedAverage += aryRecall[c] * countOfClustersDocument[c];
                summerOfFMeasureOfClustersForWeightedAverage += aryFMeasure[c] * countOfClustersDocument[c];
            }
            summerOfPrecisionOfClustersForWeightedAverage /= docNoEvaluation;
            summerOfRecallOfClustersForWeightedAverage /= docNoEvaluation;
            summerOfFMeasureOfClustersForWeightedAverage /= docNoEvaluation;

            //++++++++++

            StreamWriter swFinal = new StreamWriter("d:\\discriminative\\fuzzySimilarity\\Final.txt");
            for (int c = 0; c < clusterNo; c++)
            {
                swFinal.WriteLine(" for cluster number {0} --> Precision= {1}  Recall={2} F-Measure={3}", c + 1, aryPrecision[c], aryRecall[c], aryFMeasure[c]);
            }
            swFinal.WriteLine();
            swFinal.WriteLine(" Average Precision={0}  Average Recall={1}  Average F-Measure={2}", summerOfPrecisionOfClustersForAverage * 100, summerOfRecallOfClustersForAverage * 100, summerOfFMeasureOfClustersForAverage * 100);
            swFinal.Close();

            //============================================= THE END =============================================================================================

            System.Console.WriteLine("\n Candidate Clusters={0} & {1} with {2} and {3} value of Similarity.", candidateCluster1 + 1, candidateCluster2 + 1, aryTopTwoSimilarity[clusterNo - 1], aryTopTwoSimilarity[clusterNo - 2]);
            System.Console.WriteLine("\n docNo={0}   docNoTitleText={1}   docNoEvaluation={2}\n\n clusterNo={3}   TermNo={4}   termNoSpecificTestDoc={5}   docNoCandidated={6}", docNo, docNoEvaluation, docNoEvaluation, clusterNo, termNo, TermNoSpecificTestDoc, docNoCandidated);
            //System.Console.WriteLine(" Precision = %{0}  Recall = %{1}  F-Measure = %{2}", precision * 100, recall * 100, FMeasure * 100);
            System.Console.WriteLine("\n\nAverage Precision={0}\n\nAverage Recall={1}\n\nAverage F-Measure={2}", summerOfPrecisionOfClustersForAverage * 100, summerOfRecallOfClustersForAverage * 100, summerOfFMeasureOfClustersForAverage * 100);
            System.Console.WriteLine("\n\nAverage Weighted Precision={0}\n\nAverage Weighted Recall={1}\n\nAverage Weighted F-Measure={2}", summerOfPrecisionOfClustersForWeightedAverage * 100, summerOfRecallOfClustersForWeightedAverage * 100, summerOfFMeasureOfClustersForWeightedAverage * 100);
            System.Console.WriteLine("\n\nPRECISION1={0}   RECALL1={1}   FMEASURE1={2} \nPRECISION0={3}   RECALL0={4}   FMEASURE0={5} ", PRECISION1, RECALL1, FMEASURE1, PRECISION0, RECALL0, FMEASURE0);
            System.Console.WriteLine("\n\n F Micro={0}", FMeasure);
            System.Console.WriteLine("\n\n F Macro={0}", FMacro);
            //***************************************************************************************************************************************************
            System.Console.WriteLine("\n\n\n                    Running has been Finished!");
            System.Console.ReadKey();

        }
    }
}
