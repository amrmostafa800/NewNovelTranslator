using NovelTextProcessor;
using NovelTextProcessor.Dtos;

namespace NewNovelTranslator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void start_Click(object sender, EventArgs e)
        {
            //DocumentProcessor docprocessor = new DocumentProcessor(text.Text);

            ////var result = docprocessor.ExtractEntityNames();

            //foreach (var span in docprocessor.document) 
            //{
            //    var test = span.TokenizedValue;
            //}


            var seedData = new List<EntityName>
            {
                //new EntityName { Name = "Alice", Gender = 'F' },
                //new EntityName { Name = "Bob", Gender = 'M' },
                //new EntityName { Name = "Charlie", Gender = 'M' },
                //new EntityName { Name = "David", Gender = 'M' },
                //new EntityName { Name = "Emily", Gender = 'F' },
                //new EntityName { Name = "Felicia", Gender = 'F' },
                //new EntityName { Name = "George", Gender = 'M' },
                //new EntityName { Name = "Hannah", Gender = 'F' },
                //new EntityName { Name = "Isaac", Gender = 'M' },
                //new EntityName { Name = "Jessica", Gender = 'F' },
                //new EntityName { Name = "Nicole", Gender = 'F' },
                //new EntityName { Name = "Oliver", Gender = 'M' },
                //new EntityName { Name = "Patricia", Gender = 'F' },
                //new EntityName { Name = "Quentin", Gender = 'M' },
                //new EntityName { Name = "Rachel", Gender = 'F' },
                //new EntityName { Name = "Steven", Gender = 'M' },
                new EntityName { EnglishName = "Kevin", ArabicName = "����", Gender = 'M' },
                new EntityName { EnglishName = "Laura", ArabicName = "����", Gender = 'F' },
                new EntityName { EnglishName = "Michael", ArabicName = "�����", Gender = 'M' },
            };

            Processor processor = new Processor(text.Text, seedData.ToArray());

            MessageBox.Show("");

        }
    }
}
