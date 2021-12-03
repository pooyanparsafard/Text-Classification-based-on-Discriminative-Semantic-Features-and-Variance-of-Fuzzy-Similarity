using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;


namespace discriminative
{
    public partial class Form1 : Form
    {
        //========== Form Initializing

        Variables objectVariables = new Variables();
        Functions objectFunctions = new Functions();
        Preprocessing objectPreprocessing = new Preprocessing();

        

        public Form1()
        {
            InitializeComponent();
        }
       

        //========== Number of Train Documents - TextBox
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          int correct;
          if  (int.TryParse(textBox1.Text , out correct))
            {
             textBox2.Enabled = true;
                objectVariables.intNumberOfDocuments = int.Parse(textBox1.Text);
            }
            else
            {
                textBox2.Enabled = false;
            }
        }
        
        //========== Number of Categories - TextBox 

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int correct;
            if (int.TryParse(textBox2.Text, out correct))
            {
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                objectVariables.intNumberOfCategories = int.Parse(textBox2.Text);
            }
            else
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
            }
            textBox1.Enabled = false;
            textBox5.Enabled = false;
            button14.Enabled = true;
        }

        //========== English - Button
       
        public bool englishChoosed;
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Enabled = true;
            englishChoosed = true;
            objectVariables.aryCategory = new string[objectVariables.intNumberOfCategories];
            textBox2.Enabled = false;
        }

        //========== Persian - Button

        public bool persianChoosed;
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Enabled = true;
            persianChoosed = true;
            objectVariables.aryCategory = new string[objectVariables.intNumberOfCategories];
            textBox2.Enabled = false;     
        }

        //========== Category Name - TextBox

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

            if (textBox3.Text == "")
            {
                button16.Enabled = false;
            }
            else
            {
                button16.Enabled = true;
            }
        }

        //========== Add - Button
        
        public int categoryAdder=0;
        private void button16_Click_1(object sender, EventArgs e)
        {
            objectVariables.aryCategory[categoryAdder] = textBox3.Text;
            categoryAdder++;
            objectVariables.strAlertText =objectVariables.strAlertText + string.Format("\nThe Category {0} is {1} ",categoryAdder,textBox3.Text);
            richTextBox1.Text = objectVariables.strAlertText;
            textBox3.Text = "";
            if (categoryAdder == objectVariables.intNumberOfCategories)
            {
                button1.Enabled = true;
                button16.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                textBox3.Enabled = false;
            }
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
        }

        //========== Select the Input Text - Button

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = new DialogResult();
            OpenFileDialog open1 = new OpenFileDialog();
            open1.Title="Select the Text.txt";
            open1.Filter = "TXT files|*.txt";

            result=open1.ShowDialog();
            if (result == DialogResult.OK)
            {
                StreamReader srAryInputTextLineByLine = new StreamReader(open1.FileName);

                objectVariables.aryInputTextLineByLine = new string[objectVariables.intNumberOfDocuments];

                for (int i = 0; i < objectVariables.intNumberOfDocuments;i++ )
                {
                    objectVariables.aryInputTextLineByLine[i] = srAryInputTextLineByLine.ReadLine();
                }
                srAryInputTextLineByLine.Close();

                objectVariables.strAlertText += "\nThe Train Text is selected !";
                richTextBox1.Text = objectVariables.strAlertText;
                if (englishChoosed)
                {
                    button5.Enabled = true;
                    button4.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button6.Enabled = true;
                   
                    objectVariables.matrixDocCat=new double [objectVariables.intNumberOfDocuments,objectVariables.intNumberOfCategories];
                    objectVariables.matrixDocCat= objectFunctions.createMatrixDocCat(objectVariables.aryInputTextLineByLine, objectVariables.aryCategory);
                    objectVariables.aryInputTextLineByLine =objectFunctions.omitLabel(objectVariables.aryInputTextLineByLine);
                 
                }
                else if(persianChoosed)
                {
                    button17.Enabled = true;
                    button18.Enabled = true;
                    button19.Enabled = true;
                    button20.Enabled = true;
                    button6.Enabled = true;

                    objectVariables.matrixDocCat = new double[objectVariables.intNumberOfDocuments, objectVariables.intNumberOfCategories];
                    objectVariables.matrixDocCat = objectFunctions.createMatrixDocCat(objectVariables.aryInputTextLineByLine, objectVariables.aryCategory);
                    objectFunctions.writeMatrix(objectVariables.matrixDocCat, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfCategories, "matrixDocCat");
                    objectVariables.aryInputTextLineByLine = objectFunctions.omitLabel(objectVariables.aryInputTextLineByLine);
                }
                button1.Enabled = false;
                button27.Enabled = true;
                if (objectVariables.boolTestTextIsSelected)
                    textBox5.Enabled = false;    
                }
            open1.Dispose();
            }          
        

        //========== richTextBox1

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //========== Remove White Spaces & None Alphabeticals - Button

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < objectVariables.aryInputTextLineByLine.Length; i++ )
            {
                objectVariables.aryInputTextLineByLine[i] = objectFunctions.removeWhiteSpacesAndNonAlphabetical(objectVariables.aryInputTextLineByLine[i]);
            }
            objectVariables.strAlertText = objectVariables.strAlertText + "\nThe White Spaces && Non-Alphabetical are removed !" ;
            richTextBox1.Text = objectVariables.strAlertText;
            button5.Enabled = false;
        }

        //========== Remove the Stopwords - Button

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < objectVariables.aryInputTextLineByLine.Length; i++)
            {
                objectVariables.aryInputTextLineByLine[i] = objectPreprocessing.removeStopwords(objectVariables.aryInputTextLineByLine[i]);
            }
            objectVariables.strAlertText = objectVariables.strAlertText + "\nThe Stopwords are removed !";
            richTextBox1.Text = objectVariables.strAlertText;
            button2.Enabled = false;
        }

        //========== Stemming - Button

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
        }

        //========== Write The Documents Line by Line - Button

        private void button4_Click(object sender, EventArgs e)
        {
            objectFunctions.writeStringArrayLineByLine(objectVariables.aryInputTextLineByLine, "documentsLineByLine");
            objectVariables.strAlertText = objectVariables.strAlertText + "\nThe Documents are written line by line !";
            richTextBox1.Text = objectVariables.strAlertText;
            button4.Enabled = false;
            button5.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        //========== Remove White Spaces & None Alphabeticals Persian - Button

        private void button17_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < objectVariables.aryInputTextLineByLine.Length; i++)
            {
                objectVariables.aryInputTextLineByLine[i] = objectFunctions.removeWhiteSpacesAndNonAlphabeticalPersian(objectVariables.aryInputTextLineByLine[i]);
            }
            objectVariables.strAlertText = objectVariables.strAlertText + "\nThe White Spaces && Non-Alphabetical are removed !";
            richTextBox1.Text = objectVariables.strAlertText;
            button17.Enabled = false;
        }

        //========== Remove the Stopwords Persian - Button

        private void button20_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < objectVariables.aryInputTextLineByLine.Length; i++)
            {
                objectVariables.aryInputTextLineByLine[i] = objectPreprocessing.removeStopwords(objectVariables.aryInputTextLineByLine[i]);
            }
            objectVariables.strAlertText = objectVariables.strAlertText + "\nThe Stopwords are removed !";
            richTextBox1.Text = objectVariables.strAlertText;
            button20.Enabled = false;
        }

        //========== Stemming Persian - Button

        private void button19_Click(object sender, EventArgs e)
        {
            button19.Enabled = false;
        }

        //========== Write The Documents Line by Line Persian - Button

        private void button18_Click(object sender, EventArgs e)
        {
            objectFunctions.writeStringArrayLineByLine(objectVariables.aryInputTextLineByLine, "documentsLineByLinePersian");
            objectVariables.strAlertText = objectVariables.strAlertText + "\nThe Documents are written line by line !";
            richTextBox1.Text = objectVariables.strAlertText;
            button18.Enabled = false;
            button17.Enabled = false;
            button19.Enabled = false;
            button20.Enabled = false;
        }

        //========== Extract the Terms - Button

        private void button6_Click(object sender, EventArgs e)
        {
            objectVariables.strInputTextTermByTerm = "";
            for (int i=0 ; i<objectVariables.aryInputTextLineByLine.Length; i++)
                {
                objectVariables.strInputTextTermByTerm += objectVariables.aryInputTextLineByLine[i] + " ";
                }
            objectVariables.aryInputTextDistinctTerms = objectFunctions.extractTerms(objectVariables.strInputTextTermByTerm);
            objectVariables.intNumberOfTerms= objectVariables.aryInputTextDistinctTerms.Length;
            button7.Enabled = true;
            button8.Enabled = true;
            objectVariables.strAlertText += "\nThe Terms are Extracted ! ";
            richTextBox1.Text = objectVariables.strAlertText;
            button13.Enabled = true;
            button15.Enabled = true;
            button6.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;
            button17.Enabled = false;
            button19.Enabled = false;
            button20.Enabled = false;

        }

        //========== Sort the Terms - Button

        private void button8_Click(object sender, EventArgs e)
        {
            Array.Sort(objectVariables.aryInputTextDistinctTerms);
            objectVariables.strAlertText += "\nThe Terms are Sorted ! ";
            richTextBox1.Text = objectVariables.strAlertText;
            button8.Enabled = false;
        }

        //========== Write the Terms Line by Line - Button

         private void button7_Click(object sender, EventArgs e)
        {
            objectFunctions.writeStringArrayLineByLine(objectVariables.aryInputTextDistinctTerms, "termsLineByLine");
            objectVariables.strAlertText += "\nThe Terms are Written Line by Line ! ";
            richTextBox1.Text = objectVariables.strAlertText;
            button7.Enabled = false;
            button8.Enabled = false;
          }

        //========== Write Matrix Doc-Cat - Button

         private void button27_Click(object sender, EventArgs e)
         {
            objectFunctions.writeMatrix(objectVariables.matrixDocCat, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfCategories, "matrixDocCat");
            objectVariables.strAlertText += "\nThe Matrix Doc-Cat is written ! ";
            richTextBox1.Text = objectVariables.strAlertText;
            button27.Enabled = false;
         }
         
        //========== Create the Matrix Doc-Term - Button

         private void button13_Click(object sender, EventArgs e)
         {
             objectVariables.matrixDocTerm=new double [objectVariables.intNumberOfDocuments,objectVariables.intNumberOfTerms];
             objectVariables.matrixDocTerm=objectFunctions.createMatrixDocTerm(objectVariables.aryInputTextLineByLine, objectVariables.aryInputTextDistinctTerms);
             objectVariables.strAlertText += "\nThe Matrix Doc-Term is created ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             button9.Enabled = true;
             button12.Enabled = true;
             if (objectVariables.boolTestTextIsSelected)
             {
                 button25.Enabled = false;
                 button32.Enabled = true;
                 //--- Next Application
                 objectVariables.intNumberOfTestMultiLabels = objectFunctions.multiLabelsCounting(objectVariables.matrixDocCat,objectVariables.intNumberOfDocuments,objectVariables.intNumberOfCategories);
                 objectVariables.intNumberOfTestLabels = objectFunctions.labelsCounting(objectVariables.matrixDocCat, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfCategories);
                 objectFunctions.readyTestArrayNextApplication(objectVariables.aryInputTextLineByLine, objectVariables.aryFeatures, objectVariables.aryCategory, objectVariables.matrixDocCat, objectVariables.intNumberOfCategories, "evaluationTitleText");
                 textBox9.Text = objectVariables.intNumberOfTestMultiLabels.ToString();
                 textBox8.Text = (objectVariables.intNumberOfDocuments - objectVariables.intNumberOfTestMultiLabels).ToString();
                 objectFunctions.zeroOne(objectVariables.matrixDocCat,objectVariables.intNumberOfDocuments,objectVariables.intNumberOfCategories);
                 //---
             }
             else
             {
                 button25.Enabled = true;
             }
             button13.Enabled = false;
             button8.Enabled = false;
         }

         //========== Normalize the Matrix - Button

         private void button12_Click(object sender, EventArgs e)
         {
             objectVariables.matrixDocTerm = objectFunctions.normalizMatrix(objectVariables.matrixDocTerm, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfTerms);
             objectVariables.strAlertText += "\nThe Matrix Doc-Term is Normalized ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             button12.Enabled = false;
         }

        //========== Write Matrix Doc-Term - Button

         private void button9_Click(object sender, EventArgs e)
         {
             objectFunctions.writeMatrix(objectVariables.matrixDocTerm, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfTerms, "matrixDocTerm");
             objectVariables.strAlertText += "\nThe Matrix Doc-Term is written ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             button9.Enabled = false;
             button12.Enabled = false;
         }

        //========== Create Matrix Term-Category - Button

         private void button25_Click(object sender, EventArgs e)
         {
             objectVariables.matrixTermCategory = new double[objectVariables.intNumberOfTerms, objectVariables.intNumberOfCategories];
             objectVariables.matrixTermCategory = objectFunctions.createMatrixTermCat(objectVariables.matrixDocTerm, objectVariables.matrixDocCat, objectVariables.intNumberOfTerms, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfCategories);
             objectVariables.strAlertText += "\nThe Matrix Term-Category is created ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             button26.Enabled = true;
             button24.Enabled = true;
             button25.Enabled = false;
             button12.Enabled = false;
             button21.Enabled = true;

         }

        //========== Normalize the Matrix Term-Category - Button

         private void button26_Click(object sender, EventArgs e)
         {
             objectFunctions.normalizMatrix(objectVariables.matrixTermCategory,objectVariables.intNumberOfTerms,objectVariables.intNumberOfCategories);
             objectVariables.strAlertText += "\nThe Matrix Term-Category is Normalized ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             button26.Enabled = false;
         }

        //========== Write the Matrix Term-Category - Button

         private void button24_Click(object sender, EventArgs e)
         {
             objectFunctions.writeMatrix(objectVariables.matrixTermCategory,objectVariables.intNumberOfTerms,objectVariables.intNumberOfCategories,"matrixTermCategory");
             objectVariables.strAlertText += "\nThe Matrix Term-Category is written ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             button24.Enabled = false;
             button26.Enabled = false;
         }

        //========== Create Matrix Contingency - Button

         private void button21_Click(object sender, EventArgs e)
         {
             objectVariables.matrixContingency=new double[objectVariables.intNumberOfTerms,objectVariables.intNumberOfCategories*4];
             objectVariables.matrixContingency = objectFunctions.createMatrixContingency(objectVariables.matrixDocTerm,objectVariables.matrixDocCat,objectVariables.intNumberOfTerms,objectVariables.intNumberOfCategories,objectVariables.intNumberOfDocuments);
             objectVariables.strAlertText += "\nThe Matrix Contingency is created ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             button11.Enabled = true;
             button23.Enabled = true;
             button21.Enabled = false;
             button26.Enabled = false;
         }

        //========== Write Matrix Contingency - Button

         private void button11_Click(object sender, EventArgs e)
         {
             objectFunctions.writeMatrix(objectVariables.matrixContingency, objectVariables.intNumberOfTerms, objectVariables.intNumberOfCategories * 4, "matrixContingency");
             objectVariables.strAlertText += "\nThe Matrix Contingency is written ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             button11.Enabled = false;
         }

        //========== Create Matrix DFS - Button

         private void button23_Click(object sender, EventArgs e)
         {
             objectVariables.matrixDFS=new double[objectVariables.intNumberOfTerms,objectVariables.intNumberOfCategories];
             objectVariables.matrixDFS = objectFunctions.createMatrixDFS(objectVariables.matrixContingency,objectVariables.matrixTermCategory,objectVariables.intNumberOfTerms,objectVariables.intNumberOfCategories);
             objectVariables.strAlertText += "\nThe Matrix DFS is created ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             button22.Enabled = true;
             button10.Enabled = true;
             button23.Enabled = false;
         }

        //========== Write Matrix DFS - Button


         private void button22_Click(object sender, EventArgs e)
         {
             objectFunctions.writeMatrix(objectVariables.matrixDFS,objectVariables.intNumberOfTerms,objectVariables.intNumberOfCategories,"matrixDFS");
             objectVariables.strAlertText += "\nThe Matrix DFS is written ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             button22.Enabled = false;
         }

        //========== Creation & Writing Terms with their Max-DFS - Button

         private void button10_Click(object sender, EventArgs e)
         {
             objectVariables.aryTermsWithMaxDFS=new double[objectVariables.intNumberOfTerms];
             objectVariables.aryTermsWithMaxDFS = objectFunctions.maxOfRowsOfMatrix(objectVariables.matrixDFS,objectVariables.intNumberOfTerms,objectVariables.intNumberOfCategories);
             objectFunctions.writeDoubleArrayLineByLine(objectVariables.aryTermsWithMaxDFS,"termsWithMaxDFS");
             objectVariables.strAlertText += "\nThe Terms with their Max-DFS is created & written ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             textBox4.Enabled = true;
             button10.Enabled = false;
         }

         //========== How many Features do you want to input? - TextBox

         private void textBox4_TextChanged(object sender, EventArgs e)
         {
             int correct;
             if (int.TryParse(textBox4.Text, out correct))
             {
                 button28.Enabled = true;
                 objectVariables.intNumberOfFeatures=int.Parse(textBox4.Text);
             }
             else
             {
                 button28.Enabled = false;
             }
         }

         //========== Feature Selecting + Write - Button

         private void button28_Click(object sender, EventArgs e)
         {
             objectVariables.aryIndexesOfMaxToMinOfFeaturesDFS=new int [objectVariables.intNumberOfTerms];
             objectVariables.aryIndexesOfMaxToMinOfFeaturesDFS = objectFunctions.sortMaxToMinWithGettingIndexesDoubleArray(objectVariables.aryTermsWithMaxDFS);
             objectFunctions.writeIntArrayLineByLine(objectVariables.aryIndexesOfMaxToMinOfFeaturesDFS, "indexesOfMaxToMinOfFeaturesDFS");
             objectVariables.aryFeaturesIndexes = new int[objectVariables.intNumberOfFeatures];
             objectVariables.aryFeaturesIndexes = objectFunctions.featureIndexes(objectVariables.aryIndexesOfMaxToMinOfFeaturesDFS,objectVariables.intNumberOfFeatures);
             objectFunctions.writeIntArrayLineByLine(objectVariables.aryFeaturesIndexes,"featureIndexes");
             objectVariables.aryFeatures=new string[objectVariables.intNumberOfFeatures];
             objectVariables.aryFeatures = objectFunctions.featureSelecting(objectVariables.aryInputTextDistinctTerms,objectVariables.aryIndexesOfMaxToMinOfFeaturesDFS,objectVariables.intNumberOfFeatures);
             objectFunctions.writeStringArrayLineByLine(objectVariables.aryFeatures,"features");
             //--- Next Aplication
             objectVariables.intNumberOfTrainMultiLabels = objectFunctions.multiLabelsCounting(objectVariables.matrixDocCat, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfCategories);
             objectVariables.intNumberOfTrainLabels = objectFunctions.labelsCounting(objectVariables.matrixDocCat, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfCategories);
             objectFunctions.readyTrainArrayNextApplication(objectVariables.aryInputTextLineByLine,objectVariables.aryFeatures,objectVariables.aryCategory,objectVariables.matrixDocCat,objectVariables.intNumberOfCategories,objectVariables.intNumberOfTrainLabels,"textTitle","clusterID");

             textBox6.Text = objectVariables.intNumberOfTrainMultiLabels.ToString();
             textBox7.Text = (objectVariables.intNumberOfDocuments - objectVariables.intNumberOfTrainMultiLabels).ToString();
             //---
             objectVariables.strAlertText += "\nThe Feature Selecting is DONE ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             textBox4.Enabled = false;
             button32.Enabled = true;
             button28.Enabled = false;
         }

         //========== Create Matrix Doc-TFIDF - Button
        
         private void button32_Click(object sender, EventArgs e)
         {
             objectVariables.matrixContingency = new double[1, 1];
             objectVariables.matrixTermCategory = new double[1, 1];
             objectVariables.matrixDFS = new double[1, 1];
             objectVariables.aryInputTextLineByLine = new string[1];
             objectVariables.strInputTextTermByTerm = "";
             objectPreprocessing.stopWords = new string[1];

             if (objectVariables.boolTestTextIsSelected)
             {
                 for (int b = 0; b < objectVariables.aryFeaturesIndexes.Length;b++ )
                     objectVariables.aryFeaturesIndexes[b]=-1;

                 for (int v = 0; v < objectVariables.aryFeatures.Length; v++)
                 {
                     for (int w = 0; w < objectVariables.aryInputTextDistinctTerms.Length; w++)
                     {
                         if (objectVariables.aryFeatures[v] == objectVariables.aryInputTextDistinctTerms[w])
                         {
                             objectVariables.aryFeaturesIndexes[v] = w;
                         }
                     }
                 }
             }

             objectVariables.matrixDocTFIDF=new double [objectVariables.intNumberOfDocuments,objectVariables.intNumberOfFeatures];
             objectVariables.matrixDocTFIDF = objectFunctions.createMatrixDocTFIDF(objectVariables.matrixDocTerm,objectVariables.aryFeaturesIndexes,objectVariables.intNumberOfDocuments);
             objectFunctions.roundMatrix(objectVariables.matrixDocTFIDF,objectVariables.intNumberOfDocuments,objectVariables.intNumberOfFeatures);
             objectFunctions.writeStringArrayLineByLine(objectVariables.aryFeatures, "features");
             objectVariables.strAlertText += "\nThe Matrix Doc-TFIDF is Created ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             button31.Enabled = true;
             button30.Enabled = true; 
             button32.Enabled = false;
         }

         //========== Write Matrix Doc-TFIDF - Button

         private void button31_Click(object sender, EventArgs e)
         {
             if (objectVariables.boolTestTextIsSelected)
             {
                 objectFunctions.writeMatrix(objectVariables.matrixDocTFIDF, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfFeatures, "matrixTestDocTFIDF");
                 objectFunctions.writeStringArrayLineByLine(objectVariables.aryFeatures, "features");
                 objectVariables.strAlertText += "\nThe Matrix Doc-TFIDF is written ! ";
                 richTextBox1.Text = objectVariables.strAlertText;
                 button31.Enabled = false;
             }
             else
             {
                 objectFunctions.writeMatrix(objectVariables.matrixDocTFIDF, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfFeatures, "matrixDocTFIDF");
                 objectFunctions.writeStringArrayLineByLine(objectVariables.aryFeatures, "features");
                 objectVariables.strAlertText += "\nThe Matrix Doc-TFIDF is written ! ";
                 richTextBox1.Text = objectVariables.strAlertText;
                 button31.Enabled = false;
             }
         }

         //========== Create Matrix Feature-Feature Similarity - Button

         private void button30_Click(object sender, EventArgs e)
         {
             objectVariables.matrixFeatFeatSimilarity= new double[objectVariables.intNumberOfFeatures, objectVariables.intNumberOfFeatures];
             objectVariables.matrixFeatFeatSimilarity = objectFunctions.createMatrixFeatFeatSimilarity(objectVariables.matrixDocTFIDF,objectVariables.intNumberOfFeatures,objectVariables.intNumberOfDocuments);
             objectFunctions.roundMatrix(objectVariables.matrixFeatFeatSimilarity,objectVariables.intNumberOfFeatures,objectVariables.intNumberOfFeatures);
             objectVariables.strAlertText += "\nThe Matrix Feature-Feature is created ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             button29.Enabled = true;
             button34.Enabled = true;
             button30.Enabled = false;
         }

         //========== Write Matrix Feature-Feature Similarity - Button

         private void button29_Click(object sender, EventArgs e)
         {
             if (objectVariables.boolTestTextIsSelected)
             {
                 objectFunctions.writeMatrix(objectVariables.matrixFeatFeatSimilarity, objectVariables.intNumberOfFeatures, objectVariables.intNumberOfFeatures, "matrixTestFeatFeatSimilarity");
                 objectVariables.strAlertText += "\nThe Matrix Feature-Feature is written ! ";
                 richTextBox1.Text = objectVariables.strAlertText;
                 button29.Enabled = false;
             }
             else
             {
                 objectFunctions.writeMatrix(objectVariables.matrixFeatFeatSimilarity, objectVariables.intNumberOfFeatures, objectVariables.intNumberOfFeatures, "matrixFeatFeatSimilarity");
                 objectVariables.strAlertText += "\nThe Matrix Feature-Feature is written ! ";
                 richTextBox1.Text = objectVariables.strAlertText;
                 button29.Enabled = false;
             }
         }

         //========== Create Matrix Feat-Doc Similarity - Button

         private void button34_Click(object sender, EventArgs e)
         {       
             objectVariables.matrixDocTerm=new double [1,1];
             objectVariables.matrixFeatDocSimilarity = new double[objectVariables.intNumberOfFeatures, objectVariables.intNumberOfDocuments];
             objectVariables.matrixFeatDocSimilarity = objectFunctions.createMatrixFeatDocSimilarity(objectVariables.matrixDocTFIDF, objectVariables.matrixFeatFeatSimilarity, objectVariables.intNumberOfFeatures,objectVariables.intNumberOfDocuments);
             objectFunctions.roundMatrix(objectVariables.matrixFeatDocSimilarity,objectVariables.intNumberOfFeatures,objectVariables.intNumberOfDocuments);
             objectVariables.strAlertText += "\nThe Matrix Feat-Doc Similarity is created ! ";
             richTextBox1.Text = objectVariables.strAlertText;
             button33.Enabled = true;
             button35.Enabled = true;
             button34.Enabled = false;
         }

         //=========== Write Matrix Feat-Doc Similarity - Button

         private void button33_Click(object sender, EventArgs e)
         {
             if (objectVariables.boolTestTextIsSelected)
             {
                 objectFunctions.writeMatrix(objectVariables.matrixFeatDocSimilarity, objectVariables.intNumberOfFeatures, objectVariables.intNumberOfDocuments, "matrixTestFeatDocSimilarity");
                 objectVariables.strAlertText += "\nThe Matrix Feat-Doc Similarity is written ! ";
                 richTextBox1.Text = objectVariables.strAlertText;
                 button33.Enabled = false;
             }
             else
             {
                 objectFunctions.writeMatrix(objectVariables.matrixFeatDocSimilarity, objectVariables.intNumberOfFeatures, objectVariables.intNumberOfDocuments, "matrixFeatDocSimilarity");
                 objectVariables.strAlertText += "\nThe Matrix Feat-Doc Similarity is written ! ";
                 richTextBox1.Text = objectVariables.strAlertText;
                 button33.Enabled = false;
             }
         }

         //========== Creat && Write matrix Doc-TFID Modiefied - Button

         private void button35_Click(object sender, EventArgs e)
         {
             if (objectVariables.boolTestTextIsSelected)
             {
                 objectVariables.matrixDocTFIDFModified = new double[objectVariables.intNumberOfDocuments, objectVariables.intNumberOfFeatures];
                 objectVariables.matrixDocTFIDFModified = objectFunctions.createMatrixDocTFIDFModified(objectVariables.matrixDocTFIDF, objectVariables.matrixFeatDocSimilarity, objectVariables.matrixDocCat, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfFeatures, objectVariables.intNumberOfCategories, objectVariables.boolTestTextIsSelected);
                 objectFunctions.roundMatrix(objectVariables.matrixDocTFIDFModified,objectVariables.intNumberOfDocuments,objectVariables.intNumberOfFeatures);
                 objectFunctions.writeMatrix(objectVariables.matrixDocTFIDFModified, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfFeatures, "matrixTestDocTFIDFModified");
                 objectVariables.strAlertText += "\nThe Matrix Doc-TFIDFModified is created && written ! ";
                 richTextBox1.Text = objectVariables.strAlertText;
                 button35.Enabled = false;
                 textBox5.Enabled = true;
                 button37.Enabled = true;
             }
             else
             {
                 objectVariables.matrixDocTFIDFModified = new double[objectVariables.intNumberOfDocuments, objectVariables.intNumberOfFeatures];
                 objectVariables.matrixDocTFIDFModified = objectFunctions.createMatrixDocTFIDFModified(objectVariables.matrixDocTFIDF, objectVariables.matrixFeatDocSimilarity,objectVariables.matrixDocCat, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfFeatures, objectVariables.intNumberOfCategories, objectVariables.boolTestTextIsSelected);
                 objectFunctions.roundMatrix(objectVariables.matrixDocTFIDFModified, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfFeatures);
                 objectFunctions.writeMatrix(objectVariables.matrixDocTFIDFModified, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfFeatures, "matrixDocTFIDFModified");
                 objectVariables.strAlertText += "\nThe Matrix Doc-TFIDFModified is created && written ! ";
                 richTextBox1.Text = objectVariables.strAlertText;
                 button35.Enabled = false;
                 textBox5.Enabled = true;
                 button37.Enabled = true;
             }
         }

         //========== Write Matrix Doc-TFIDF Modified with Targets - Button

         private void button37_Click(object sender, EventArgs e)
         {
             if (objectVariables.boolTestTextIsSelected)
             {
                 objectFunctions.writeMatrixTargets(objectVariables.matrixDocTFIDFModified, objectVariables.matrixDocCat, objectVariables.aryCategory, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfFeatures, objectVariables.intNumberOfCategories, "matrixTestDocTFIDFModifiedTargets", "targets", "A");
                 objectVariables.strAlertText += "\nThe Matrix Doc-TFIDFModifiedTargets is written ! ";
                 richTextBox1.Text = objectVariables.strAlertText;
                 button37.Enabled = false;
             }
             else
             {
                 objectFunctions.writeMatrixTargets(objectVariables.matrixDocTFIDFModified, objectVariables.matrixDocCat, objectVariables.aryCategory, objectVariables.intNumberOfDocuments, objectVariables.intNumberOfFeatures, objectVariables.intNumberOfCategories, "matrixDocTFIDFModifiedTargets", "targets", "A");
                 objectVariables.strAlertText += "\nThe Matrix Doc-TFIDFModifiedTargets is written ! ";
                 richTextBox1.Text = objectVariables.strAlertText;
                 button37.Enabled = false;
             }
         }

         //========== Number of Test Documents - TextBox5

         private void textBox5_TextChanged(object sender, EventArgs e)
         {
             int correct;
             if (int.TryParse(textBox5.Text, out correct))
             {
                 objectVariables.intNumberOfDocuments = int.Parse(textBox5.Text);
                 button1.Enabled = true;
                 objectVariables.boolTestTextIsSelected = true;
                 button7.Enabled = false;
                 button27.Enabled = false;
                 button9.Enabled = false;
                 button24.Enabled = false;
                 button11.Enabled = false;
                 button31.Enabled = false;
                 button29.Enabled = false;
                 button33.Enabled = false;
             }
             else
             {
                 button1.Enabled = false;
             }  
         }

         //========== intNumberOfDocuments - Button

         private void button14_Click(object sender, EventArgs e)
         {
             textBoxIntNumberOfDocuments.Text = objectVariables.intNumberOfDocuments.ToString();
         }

         //========== intNumberOfTerms - Button

         private void button15_Click(object sender, EventArgs e)
         {
             textBoxintNumberOfTerms.Text = objectVariables.intNumberOfTerms.ToString();
         }

         //========== Number of Train Multilabels - TextBox

         private void textBox6_TextChanged(object sender, EventArgs e)
         {
         }

         //=========== Number of Train Singlelabels - TextBox

         private void textBox7_TextChanged(object sender, EventArgs e)
         {
         }

         //========== Number of Test Multilabels - TextBox

         private void textBox9_TextChanged(object sender, EventArgs e)
         {
         }

         //=========== Number of Test Singlelabels - TextBox

         private void textBox8_TextChanged(object sender, EventArgs e)
         {
         }

         //============

         private void Form1_Load(object sender, EventArgs e)
         {

         }

 

      

   

  





      


        //========== End of Form

    }
}
