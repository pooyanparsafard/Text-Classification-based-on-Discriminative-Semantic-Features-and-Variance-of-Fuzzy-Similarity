using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace discriminative
{
   public  class  Variables
    {



        public  int intNumberOfDocuments {set; get;}

       //==========

        public  string[] aryInputTextLineByLine { set; get; }

       //==========

        public  string strAlertText { set; get; }

       //==========

        public  int intNumberOfCategories { set; get; }

       //==========

        public  string[] aryCategory { set; get; }

       //==========

        public  string strInputTextTermByTerm { set; get; }

       //==========

        public  int intNumberOfTerms { set; get; }

       //==========

        public int intNumberOfTrainLabels { set; get; }

       //==========

        public int intNumberOfTrainMultiLabels { set; get; }

        //==========

        public int intNumberOfTestLabels { set; get; }

        //==========

        public int intNumberOfTestMultiLabels { set; get; }

       //==========

        public  string[] aryInputTextDistinctTerms { set; get; }

       //==========

        public  double[,] matrixDocTerm { set; get; }

       //==========

        public  double[,] matrixDocCat { set; get; }

       //==========

        public  double[,] matrixContingency { set; get; }

       //==========

        public  double[,] matrixTermCategory { set; get; }

       //==========

        public  double[,] matrixDFS { set; get; }

       //==========

        public  double[] aryTermsWithMaxDFS { set; get; }

       //==========

        public  int intNumberOfFeatures { set; get; }

       //==========

        public  int[] aryIndexesOfMaxToMinOfFeaturesDFS { set; get; }

       //==========

        public  int[] aryFeaturesIndexes { set; get; }

       //==========

        public  string[] aryFeatures { set; get; }

       //==========

        public  double[,] matrixDocTFIDF { set; get; }

       //=========

        public  int[] aryNm { set; get; }

       //=========

        public  double[,] matrixFeatFeatSimilarity { set; get;}

       //=========

        public  double[,] matrixFeatDocSimilarity { set; get; }

       //=========

        public  double[,] matrixDocTFIDFModified { set; get; }

       //=========

        public  bool boolTestTextIsSelected { set; get; } 

       //========= 
     }
}
