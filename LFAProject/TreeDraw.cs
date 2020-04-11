using System;
using System.Drawing;
using System.Windows.Forms;

namespace LFAProject
{
    public partial class TreeDraw : Form
    {
        private readonly BTreeNode tree = null;
        private static readonly Bitmap bitmap = new Bitmap(9999, 9999);
        private readonly Graphics graphics = Graphics.FromImage(bitmap);
        
        public TreeDraw(BTreeNode root)
        {
            InitializeComponent();
            tree = root;            
            DrawTree(tree, Width*2, 50, 1000);
            pictureBox1.Image = bitmap;
        }

        
        private void Close(object sender, EventArgs e)
        {
            Close();
        }
        
        private void DrawTree(BTreeNode root, float x, float y, int distance)
        {
            if (root != null)
            {
                graphics.FillEllipse(new SolidBrush(Color.BlueViolet), new RectangleF(x, y, 50, 25));
                graphics.DrawString(root.Token, new Font("Segoe UI", 11, FontStyle.Regular),
                                    new SolidBrush(Color.White), x + 5, y + 5);

                if (root.left != null)
                {
                    graphics.DrawLine(new Pen(Color.BlueViolet), x + 15, y + 15, x - distance + 15, y + 65);
                    DrawTree(root.left, x - distance, y + 50, distance / 2);
                }

                if (root.right != null)
                {
                    graphics.DrawLine(new Pen(Color.BlueViolet), x + 15, y + 15, x + distance + 15, y + 65);
                    DrawTree(root.right, x + distance, y + 50, distance);
                }
            }
        }        
    }
}
