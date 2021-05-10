using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageFilter
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
            FilterKernels = LoadDefaultKernels();
        }

        private List<Kernel> LoadDefaultKernels()
        {
            List<Kernel> tmp = new List<Kernel>();
            tmp.Add(new Kernel(Constants.BlurKernel, "Blur"));
            tmp.Add(new Kernel(Constants.GaussianBlurKernel, "Gaussian Blur"));
            tmp.Add(new Kernel(Constants.SharpenKernel, "Sharpen"));
            tmp.Add(new Kernel(Constants.EdgeDetectionKernel, "Edge Detection"));
            tmp.Add(new Kernel(Constants.EmbossKernel, "Emboss"));
            tmp.Add(new Kernel(Constants.Identity, "Custom1"));
            tmp.Add(new Kernel(Constants.Identity, "Custom2"));
            custom1ViewBtn.Text = "Create";
            custom2ViewBtn.Text = "Create";
            customFilter1Btn.Enabled = false;
            customFilter2Btn.Enabled = false;
            SelectedKernel = null;
            GreyscaleFlag = false;
            return tmp;
        }

        public bool GreyscaleFlag {get; set;}

        public Bitmap OriginalImage { get; set; }
        public Bitmap FilteredImage { get; set; }

        public List<Kernel> FilterKernels { get; set; }
        public Kernel SelectedKernel { get; set; }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.bmp, *.png) | *.jpg; *.jpeg; *.bmp; *.png";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                OriginalImage = new Bitmap(fileDialog.FileName);
                FilteredImage = OriginalImage;
                pictureBox1.Image = FilteredImage;
            }
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "JPG Image|*.jpg|Bitmap Image|*.bmp|PNG Image|*.png";
            fileDialog.Title = "Save an Image File";
            fileDialog.ShowDialog();

            if (fileDialog.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)fileDialog.OpenFile();
                switch (fileDialog.FilterIndex)
                {
                    case 1:
                        FilteredImage.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        FilteredImage.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        FilteredImage.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                }
            }
        }

        private void editKernelBox_Click(object sender, EventArgs e)
        {
            if(editKernelBox.Checked)
            {
                kernelColumnSizeEditor.Enabled = true;
                kernelRowSizeEditor.Enabled = true;
                /*for(int i = 0; i < 9; i++)
                {
                    for(int j = 0; j < 9; j++)
                    {
                        TextBox Coefficient = (kernelMatrix.GetControlFromPosition(j, i) as TextBox);
                        Coefficient.Enabled = true;
                    }
                }*/
                for (int i = 4 - SelectedKernel.Heigth / 2, x = 0; i < 4 + SelectedKernel.Heigth / 2 + 1; i++, x++)
                {
                    for (int j = 4 - SelectedKernel.Width / 2, y = 0; j < 4 + SelectedKernel.Width / 2 + 1; j++, y++)
                    {
                        TextBox Coefficient = (kernelMatrix.GetControlFromPosition(j, i) as TextBox);
                        Coefficient.Enabled = true;
                        Coefficient.ReadOnly = false;
                    }
                }
            }
            else
            {
                kernelColumnSizeEditor.Enabled = false;
                kernelRowSizeEditor.Enabled = false;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        TextBox Coefficient = (kernelMatrix.GetControlFromPosition(j, i) as TextBox);
                        Coefficient.Enabled = false;
                    }
                }
                for (int i = 4 - SelectedKernel.Heigth / 2, x = 0; i < 4 + SelectedKernel.Heigth / 2 + 1; i++, x++)
                {
                    for (int j = 4 - SelectedKernel.Width / 2, y = 0; j < 4 + SelectedKernel.Width / 2 + 1; j++, y++)
                    {
                        TextBox Coefficient = (kernelMatrix.GetControlFromPosition(j, i) as TextBox);
                        Coefficient.Enabled = true;
                        Coefficient.ReadOnly = true;
                    }
                }
            }
        }

        private bool validKernelSize(int number)
        {
            if (number >= 1 && number <= 9 && number % 2 == 1)
                return true;
            return false;
        }
        
        private void reloadVisibleMatrix()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TextBox Coefficient = (kernelMatrix.GetControlFromPosition(j, i) as TextBox);
                    Coefficient.Enabled = false;
                }
            }
            for (int i = 4 - SelectedKernel.Heigth / 2, x = 0; i < 4 + SelectedKernel.Heigth / 2 + 1; i++, x++)
                for (int j = 4 - SelectedKernel.Width / 2, y = 0; j < 4 + SelectedKernel.Width / 2 + 1; j++, y++)
                {
                    (kernelMatrix.GetControlFromPosition(j, i) as TextBox).Enabled = true;
                }
        }

        private void matrixCleanup()
        {
            for(int i = 0; i < 9; i++)
                {
                for (int j = 0; j < 9; j++)
                {
                    TextBox Coefficient = (kernelMatrix.GetControlFromPosition(j, i) as TextBox);
                    if (Coefficient.Enabled == false)
                        Coefficient.Text = "";
                }
            }
        }

        private void kernelColumnSizeEditor_ValueChanged(object sender, EventArgs e)
        {
            if (SelectedKernel == null)
                return;
            int value = decimal.ToInt32(kernelColumnSizeEditor.Value);
            if (validKernelSize(value))
                SelectedKernel.Heigth = value;
            else
            {
                MessageBox.Show("Kernel width must be an odd number in range [1, 9]");
                return;
            }
            reloadVisibleMatrix();
            matrixCleanup();
        }

        private void kernelRowSizeEditor_ValueChanged(object sender, EventArgs e)
        {
            if (SelectedKernel == null)
                return;
            int value = decimal.ToInt32(kernelRowSizeEditor.Value);
            if (validKernelSize(value))
            {
                SelectedKernel.Width = value;
            }
            else
            {
                MessageBox.Show("Kernel heigth must be an odd number in range [1, 9]");
                return;
            }
            reloadVisibleMatrix();
            matrixCleanup();
        }

        private void autoModifyAnchorX()
        {
            if (SelectedKernel == null)
                return;
            int value = decimal.ToInt32(anchorXBox.Value);
            int max = decimal.ToInt32(kernelRowSizeEditor.Value);
            if (value >= max)
            {
                anchorXBox.Value = max- 1;
            }
        }

        private void autoModifyAnchorY()
        {
            if (SelectedKernel == null)
                return;
            int value = decimal.ToInt32(anchorYEditor.Value);
            int max = decimal.ToInt32(kernelColumnSizeEditor.Value);
            if (value >= max)
            {
                anchorYEditor.Value = max - 1;
            }
        }

        private void anchorXBox_ValueChanged(object sender, EventArgs e)
        {
            autoModifyAnchorX();
        }

        private void anchorYEditor_ValueChanged(object sender, EventArgs e)
        {
            autoModifyAnchorY();
        }

        private void funFilInversionBtn_Click(object sender, EventArgs e)
        {
            try
            {
                FilteredImage = FunctionalFilters.Filter(FilteredImage, FunctionalFilters.Invert);
            }
            catch
            {
                MessageBox.Show("No Image Loaded!");
            }
            pictureBox1.Image = FilteredImage;
        }

        private void funFilBrightnessBtn_Click(object sender, EventArgs e)
        {
            try
            {
                FilteredImage = FunctionalFilters.Filter(FilteredImage, FunctionalFilters.AdjustBrightness);
            }
            catch
            {
                MessageBox.Show("No Image Loaded!");
            }
            pictureBox1.Image = FilteredImage;
        }

        private void funFilGammaBtn_Click(object sender, EventArgs e)
        {
            try
            {
                FilteredImage = FunctionalFilters.Filter(FilteredImage, FunctionalFilters.AdjustGamma);
            }
            catch
            {
                MessageBox.Show("No Image Loaded!");
            }
            pictureBox1.Image = FilteredImage;
        }

        private void funFilContrastBtn_Click(object sender, EventArgs e)
        {
            try
            {
                FilteredImage = FunctionalFilters.Filter(FilteredImage, FunctionalFilters.AdjustContrast);
            }
            catch
            {
                MessageBox.Show("No Image Loaded!");
            }
            pictureBox1.Image = FilteredImage;
        }

        private void convFilBlurBtn_Click(object sender, EventArgs e)
        {
            try
            {
                FilteredImage = ConvolutionalFilters.Filter(FilteredImage, ConvolutionalFilters.FindKernelByName(FilterKernels, "Blur"));
            }
            catch
            {
                MessageBox.Show("No Image Loaded!");
            }
            pictureBox1.Image = FilteredImage;
        }

        private void convFilGaussBtn_Click(object sender, EventArgs e)
        {
            try
            {
                FilteredImage = ConvolutionalFilters.Filter(FilteredImage, ConvolutionalFilters.FindKernelByName(FilterKernels, "Gaussian Blur"));
            }
            catch
            {
                MessageBox.Show("No Image Loaded!");
            }
            pictureBox1.Image = FilteredImage;
        }

        private void convFilEdgeDetBtn_Click(object sender, EventArgs e)
        {
            try
            {
                FilteredImage = ConvolutionalFilters.Filter(FilteredImage, ConvolutionalFilters.FindKernelByName(FilterKernels, "Edge Detection"));
            }
            catch
            {
                MessageBox.Show("No Image Loaded!");
            }
            pictureBox1.Image = FilteredImage;
        }

        private void convFilSharpenBtn_Click(object sender, EventArgs e)
        {
            try
            {
                FilteredImage = ConvolutionalFilters.Filter(FilteredImage, ConvolutionalFilters.FindKernelByName(FilterKernels, "Sharpen"));
            }
            catch
            {
                MessageBox.Show("No Image Loaded!");
            }
            pictureBox1.Image = FilteredImage;
        }

        private void convFilEmbossBtn_Click(object sender, EventArgs e)
        {
            try
            {
                FilteredImage = ConvolutionalFilters.Filter(FilteredImage, ConvolutionalFilters.FindKernelByName(FilterKernels, "Emboss"));
            }
            catch
            {
                MessageBox.Show("No Image Loaded!");
            }
            pictureBox1.Image = FilteredImage;
        }

        private void toggleOptionsBox()
        {
            if (optionsGroupBox.Visible)
            {
                optionsGroupBox.Visible = false;
                resetAllInputs();

            }
            else
            {
                optionsGroupBox.Visible = true;
                divisorInput.Text = SelectedKernel.Divisor.ToString();
                offsetInput.Text = SelectedKernel.Offset.ToString();
                anchorXBox.Value = SelectedKernel.Anchor.X;
                anchorYEditor.Value = SelectedKernel.Anchor.Y;
                kernelRowSizeEditor.Value = SelectedKernel.Width;
                kernelColumnSizeEditor.Value = SelectedKernel.Heigth;
                
            }
            
            divisorComputationBox.Checked = true;
            divisorInput.Enabled = false;
            reloadVisibleMatrix();
            matrixCleanup();
        }

        private void enterEditMode(object sender, EventArgs e)
        {
            string name = (sender as Button).Name;
            switch(name)
            {
                case "blurViewBtn":
                    SelectedKernel = new Kernel(ConvolutionalFilters.FindKernelByName(FilterKernels, "Blur"));
                    break;
                case "gaussViewBtn":
                    SelectedKernel = new Kernel(ConvolutionalFilters.FindKernelByName(FilterKernels, "Gaussian Blur"));
                    break;
                case "edgeViewBtn":
                    SelectedKernel = new Kernel(ConvolutionalFilters.FindKernelByName(FilterKernels, "Edge Detection"));
                    break;
                case "sharpenViewBtn":
                    SelectedKernel = new Kernel(ConvolutionalFilters.FindKernelByName(FilterKernels, "Sharpen"));
                    break;
                case "embossViewBtn":
                    SelectedKernel = new Kernel(ConvolutionalFilters.FindKernelByName(FilterKernels, "Emboss"));
                    break;
                case "custom1ViewBtn":
                    SelectedKernel = new Kernel(ConvolutionalFilters.FindKernelByName(FilterKernels, "Custom1"));
                    break;
                case "custom2ViewBtn":
                    SelectedKernel = new Kernel(ConvolutionalFilters.FindKernelByName(FilterKernels, "Custom2"));
                    break;
                default:
                    MessageBox.Show($"Unknown Sender: {name}");
                    break;
            }

            prepareMatrix();
            toggleOptionsBox();

        }

        private void reloadImageBtn_Click(object sender, EventArgs e)
        {
            FilteredImage = OriginalImage;
            pictureBox1.Image = FilteredImage;
            GreyscaleFlag = false;
        }

        private void reloadDefaultFiltersBtn_Click(object sender, EventArgs e)
        {
            FilterKernels = LoadDefaultKernels();
        }

        private void prepareMatrix()
        {

            foreach(var control in kernelMatrix.Controls)
            {
                (control as TextBox).Enabled = false;
            }
            for (int i = 4 - SelectedKernel.Heigth / 2, x = 0; i < 4 + SelectedKernel.Heigth / 2 + 1; i++, x++)
                for (int j = 4 - SelectedKernel.Width / 2, y = 0 ; j < 4 + SelectedKernel.Width / 2 + 1; j++, y++)
                {
                    (kernelMatrix.GetControlFromPosition(j, i) as TextBox).Enabled = true;
                    (kernelMatrix.GetControlFromPosition(j, i) as TextBox).Text = $"{SelectedKernel.Matrix[x, y]}";
                }
                    
        }

        private bool checkOffset()
        {
            int input;
            if (!int.TryParse(offsetInput.Text, out input))
                return false;
            return true;
        }

        private bool checkDivisor()
        {
            int input;
            if (!int.TryParse(divisorInput.Text, out input))
                return false;
            return true;
        }

        private bool automaticDivisorComputation()
        {
            return divisorComputationBox.Checked;
        }

        private bool checkRowSize()
        {
            int input = decimal.ToInt32(kernelRowSizeEditor.Value);
            return input >= 1 && input <= 9 && input % 2 == 1 ? true : false;
        }

        private bool checkColumnSize()
        {
            int input = decimal.ToInt32(kernelColumnSizeEditor.Value);
            return input >= 1 && input <= 9 && input % 2 == 1 ? true : false;
        }

        private bool checkMatrix()
        {
            for (int i = 4 - SelectedKernel.Heigth / 2, x = 0; i < 4 + SelectedKernel.Heigth / 2 + 1; i++, x++)
                for (int j = 4 - SelectedKernel.Width / 2, y = 0; j < 4 + SelectedKernel.Width / 2 + 1; j++, y++)
                {
                    int input;
                    if (kernelMatrix.GetControlFromPosition(j, i).Text == "")
                        return false;
                    if (!int.TryParse((kernelMatrix.GetControlFromPosition(j, i) as TextBox).Text, out input))
                        return false;
                }
            return true;
        }
        
        private bool checkAnchor()
        {
            int input;
            
            input = decimal.ToInt32(anchorXBox.Value);
            if (anchorXBox.Value >= SelectedKernel.Width)
                return false;

            input = decimal.ToInt32(anchorYEditor.Value);
            if (anchorYEditor.Value >= SelectedKernel.Heigth)
                return false;

            return true;
        }

        private bool validateAllFields()
        {
            return checkMatrix() && checkColumnSize() && checkRowSize() && checkOffset() && (automaticDivisorComputation() || checkDivisor()) && checkAnchor() ? true : false;
        }

        private void saveChangesButton_Click(object sender, EventArgs e)
        {
            if (!validateAllFields())
            {
                MessageBox.Show("Not all fields have been filled correctly!");
                return;
            }

            int[,] tmp = new int[SelectedKernel.Heigth, SelectedKernel.Width];
            for (int i = 4 - SelectedKernel.Heigth / 2, x = 0; i < 4 + SelectedKernel.Heigth / 2 + 1; i++, x++)
                for (int j = 4 - SelectedKernel.Width / 2, y = 0; j < 4 + SelectedKernel.Width / 2 + 1; j++, y++)
                {
                    int input;
                    int.TryParse((kernelMatrix.GetControlFromPosition(j, i) as TextBox).Text, out input);
                    tmp[x, y] = input;
                    //MessageBox.Show($"{(kernelMatrix.GetControlFromPosition(j, i) as TextBox).Text} at {j} {i} == {x} {y}");
                }

            
            for (int i = 0; i < SelectedKernel.Heigth; i++)
                for (int j = 0; j < SelectedKernel.Width; j++)
                {

                    SelectedKernel.Matrix[i, j] = tmp[i, j];
                }

            SelectedKernel.Anchor.X = decimal.ToInt32(anchorXBox.Value);
            SelectedKernel.Anchor.Y = decimal.ToInt32(anchorYEditor.Value);

            int value;
            if (automaticDivisorComputation())
                SelectedKernel.ComputeDivisor();
            else
            {
                int.TryParse(divisorInput.Text, out value);
                SelectedKernel.Divisor = value;
            }
            int.TryParse(offsetInput.Text, out value);
            SelectedKernel.Offset = value;

            Kernel tmpKernel = ConvolutionalFilters.FindKernelByName(FilterKernels, SelectedKernel.Name);
            tmpKernel.Heigth = SelectedKernel.Heigth;
            tmpKernel.Width = SelectedKernel.Width;
            tmpKernel.Anchor = SelectedKernel.Anchor;
            tmpKernel.Offset = SelectedKernel.Offset;
            tmpKernel.Divisor = SelectedKernel.Divisor;
            tmpKernel.Matrix = SelectedKernel.Matrix;

            toggleOptionsBox();
            matrixCleanup();

            if(SelectedKernel.Name == "Custom1")
            {
                custom1ViewBtn.Text = "View";
                customFilter1Btn.Enabled = true;
            }
            else if (SelectedKernel.Name == "Custom2")
            {
                custom2ViewBtn.Text = "View";
                customFilter2Btn.Enabled = true;
            }

           
        }

        private void resetAllInputs()
        {
            anchorXBox.Value = 0;
            anchorYEditor.Value = 0;
            offsetInput.Text = "0";
            divisorInput.Text = "0";
            matrixCleanup();
        }

        private void discardChangesButton_Click(object sender, EventArgs e)
        {
            toggleOptionsBox();
        }

        private void divisorComputationBox_Click(object sender, EventArgs e)
        {
            if (divisorComputationBox.Checked)
                divisorInput.Enabled = false;
            else
                divisorInput.Enabled = true;
        }

        private void customFilter1Btn_Click(object sender, EventArgs e)
        {
            try
            {
                FilteredImage = ConvolutionalFilters.Filter(FilteredImage, ConvolutionalFilters.FindKernelByName(FilterKernels, "Custom1"));
            }
            catch
            {
                MessageBox.Show("No Image Loaded!");
            }
            pictureBox1.Image = FilteredImage;
        }

        private void customFilter2Btn_Click(object sender, EventArgs e)
        {
            try
            {
                FilteredImage = ConvolutionalFilters.Filter(FilteredImage, ConvolutionalFilters.FindKernelByName(FilterKernels, "Custom2"));
            }
            catch
            {
                MessageBox.Show("No Image Loaded!");
            }
            pictureBox1.Image = FilteredImage;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Done By\nŁukasz Zalewski\nAssigned Algorithms: Error Diffusion & Popularity Algorithm");
        }

        private void applyGreyscaleButton_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap newImage = new Bitmap(FilteredImage);
                for (int i = 0; i < newImage.Width; i++)
                {
                    for (int j = 0; j < newImage.Height; j++)
                    {
                        Color oldColor = newImage.GetPixel(i, j);

                        int oldR = oldColor.R;
                        int oldG = oldColor.G;
                        int oldB = oldColor.B;

                        byte newR = (byte)(oldR * 0.3 + oldG * 0.3 + oldB * 0.3);
                        byte newG = (byte)(oldR * 0.3 + oldG * 0.3 + oldB * 0.3);
                        byte newB = (byte)(oldR * 0.3 + oldG * 0.3 + oldB * 0.3);

                        Color newColor = Color.FromArgb(newR, newG, newB);
                        newImage.SetPixel(i, j, newColor);
                    }
                }
                FilteredImage = newImage;
            }
            catch
            {
                MessageBox.Show("No Image Loaded!");
            }
            pictureBox1.Image = FilteredImage;
            GreyscaleFlag = true;

        }

        private void popAlgInput_TextChanged(object sender, EventArgs e)
        {
            uint val;
            if (uint.TryParse((sender as TextBox).Text, out val))
                popAlgorithmButton.Enabled = true;
            else popAlgorithmButton.Enabled = false;
        }

        private void popAlgorithmButton_Click(object sender, EventArgs e)
        {
            int val;
            if (int.TryParse(popAlgInput.Text, out val))
            {
                PopularityAlgorithm filter = new PopularityAlgorithm(val);
                FilteredImage = filter.Apply(FilteredImage);
            }
            pictureBox1.Image = FilteredImage;
        }

        private void floydDiff_Click(object sender, EventArgs e)
        {
            if(!CanUseErrorDiffusion())
            {
                MessageBox.Show("Bad Parameters!");
                return;
            }
            uint R, G, B;
            if(!GreyscaleFlag)
            {
                uint.TryParse(RedInput.Text, out R);
                uint.TryParse(GedInput.Text, out G);
                uint.TryParse(BedInput.Text, out B);
            }
            else
            {
                uint.TryParse(greyscaleInput.Text, out R);
                uint.TryParse(greyscaleInput.Text, out G);
                uint.TryParse(greyscaleInput.Text, out B);
            }
            ErrorDiffusionFilters.FloydAndSteinbergFilter filter = new ErrorDiffusionFilters.FloydAndSteinbergFilter(R, G, B);
            FilteredImage = filter.Apply(FilteredImage);
            pictureBox1.Image = FilteredImage;
        }

        private void atkinsonDiff_Click(object sender, EventArgs e)
        {
            if (!CanUseErrorDiffusion())
            {
                MessageBox.Show("Bad Parameters!");
                return;
            }
            uint R, G, B;
            if (!GreyscaleFlag)
            {
                uint.TryParse(RedInput.Text, out R);
                uint.TryParse(GedInput.Text, out G);
                uint.TryParse(BedInput.Text, out B);
            }
            else
            {
                uint.TryParse(greyscaleInput.Text, out R);
                uint.TryParse(greyscaleInput.Text, out G);
                uint.TryParse(greyscaleInput.Text, out B);
            }
            ErrorDiffusionFilters.AtkinsonFilter filter = new ErrorDiffusionFilters.AtkinsonFilter(R, G, B);
            FilteredImage = filter.Apply(FilteredImage);
            pictureBox1.Image = FilteredImage;
        }

        private void burkerDiff_Click(object sender, EventArgs e)
        {
            if (!CanUseErrorDiffusion())
            {
                MessageBox.Show("Bad Parameters!");
                return;
            }
            uint R, G, B;
            if (!GreyscaleFlag)
            {
                uint.TryParse(RedInput.Text, out R);
                uint.TryParse(GedInput.Text, out G);
                uint.TryParse(BedInput.Text, out B);
            }
            else
            {
                uint.TryParse(greyscaleInput.Text, out R);
                uint.TryParse(greyscaleInput.Text, out G);
                uint.TryParse(greyscaleInput.Text, out B);
            }
            ErrorDiffusionFilters.BurkesFilter filter = new ErrorDiffusionFilters.BurkesFilter(R, G, B);
            FilteredImage =  filter.Apply(FilteredImage);
            pictureBox1.Image = FilteredImage;
        }

        private void stuckyDiff_Click(object sender, EventArgs e)
        {
            if (!CanUseErrorDiffusion())
            {
                MessageBox.Show("Bad Parameters!");
                return;
            }
            uint R, G, B;
            if (!GreyscaleFlag)
            {
                uint.TryParse(RedInput.Text, out R);
                uint.TryParse(GedInput.Text, out G);
                uint.TryParse(BedInput.Text, out B);
            }
            else
            {
                uint.TryParse(greyscaleInput.Text, out R);
                uint.TryParse(greyscaleInput.Text, out G);
                uint.TryParse(greyscaleInput.Text, out B);
            }
            ErrorDiffusionFilters.StuckyFilter filter = new ErrorDiffusionFilters.StuckyFilter(R, G, B);
            FilteredImage = filter.Apply(FilteredImage);
            pictureBox1.Image = FilteredImage;
        }

        private void sierraDiff_Click(object sender, EventArgs e)
        {
            if (!CanUseErrorDiffusion())
            {
                MessageBox.Show("Bad Parameters!");
                return;
            }
            uint R, G, B;
            if (!GreyscaleFlag)
            {
                uint.TryParse(RedInput.Text, out R);
                uint.TryParse(GedInput.Text, out G);
                uint.TryParse(BedInput.Text, out B);
            }
            else
            {
                uint.TryParse(greyscaleInput.Text, out R);
                uint.TryParse(greyscaleInput.Text, out G);
                uint.TryParse(greyscaleInput.Text, out B);
            }
            ErrorDiffusionFilters.SierraFilter filter = new ErrorDiffusionFilters.SierraFilter(R, G, B);
            FilteredImage =  filter.Apply(FilteredImage);
            pictureBox1.Image = FilteredImage;
        }

        private void originalImageButton_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = OriginalImage;
        }

        private void filteredImageButton_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = FilteredImage;
        }

        private void toggleOptionsForErrorDiffusion()
        {
            if(GreyscaleFlag)
            {
                ee.Enabled = true;
                RedInput.Enabled = false;
                e2e.Enabled = false;
                r23r23.Enabled = false;
            }
            else
            {
                ee.Enabled = false;
                RedInput.Enabled = true;
                e2e.Enabled = true;
                r23r23.Enabled = true;
            }
        }

        public bool CanUseErrorDiffusion()
        {
            uint val;
            if(GreyscaleFlag)
            {
                if (!uint.TryParse(greyscaleInput.Text, out val))
                        return false;
                if (val < 2)
                    return false;
                return true;
            }
            else
            {
                if (!uint.TryParse(RedInput.Text, out val))
                    return false;
                if (val < 2)
                    return false;
                if (!uint.TryParse(GedInput.Text, out val))
                    return false;
                if (val < 2)
                    return false;
                if (!uint.TryParse(BedInput.Text, out val))
                    return false;
                if (val < 2)
                    return false;
                return true;
            }
            return true;
        }
    }
}
