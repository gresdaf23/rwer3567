using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
namespace LittleTetris{
    public partial class Form1 : Form{
        public const int width = 15, height = 25, k = 15;
        public int[,] shape = new int[2, 4];
        public int[,] field = new int[width, height];
        public Bitmap bitfield = new Bitmap(k * (width + 1) + 1, k * (height + 3) + 1);
        public Form1(){
            InitializeComponent();            
            SetShape();
        }
        public void FillField(){
            var gr = Graphics.FromImage(bitfield);
            gr.Clear(Color.Black);
            gr.DrawRectangle(Pens.Red, k, k, (width - 1) * k, (height - 1) * k);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++bh)
                    gr.FillRectangle(Brushes.Green, i * k, j * k, (k - 1) * field[i, j], (k - 1) * field[i, j]);
            for (int i = 0; i < 4; i++)
                gr.FillRectangle(Brushes.Red, shape[1, i] * k, shape[0, i] * k, k - 1, k - 1);
            FieldPictureBox.Image = bitfield;
        }
        private void TickTimer_Tick(object sender, EventArgs e){
            if (field[8, 4] == 1)
                Environment.Exit(0);              
            foreach(int i in (from i in Enumerable.Range(0, field.GetLength(1)) where (Enumerable.Range(0, field.GetLength(0)).Select(j => field[j, i]).Sum() >= width - 1) select i).ToArray().Take(1)) 
                for (int k = i; k > 1; k--)
                    for (int l = 1; l < width; l++)
                        field[l, k] = field[l, k - 1];
            Move(0, 1);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e){
           switch (e.KeyCode){
            case Keys.A: Move(-1, 0); break;
            case Keys.D: Move(1, 0); break; 
            case Keys.W:               
                var shapeT = new int[2, 4];
                Array.Copy(shape, shapeT, shape.Length);
                int maxx = Enumerable.Range(0, 4).Select(j => shape[1, j]).ToArray().Max();
                int maxy = Enumerable.Range(0, 4).Select(j => shape[0, j]).ToArray().Max();
                for (int i = 0; i < 4; i++) { 
                    int temp = shape[0, i];
                    shape[0, i] = maxy - (maxx - shape[1, i]) - 1;
                    shape[1, i] = maxx - (3 - (maxy - temp)) + 1;
                }
                if (FindMistake())
                    Array.Copy(shapeT, shape, shape.Length);
                break;
            case Keys.S: TickTimer.Interval = 50; break;
           }
        }
        public void SetShape(){
            Random x = new Random(DateTime.Now.Millisecond);
            switch (x.Next(7)){
                case 0: shape = new int[,] { { 2, 3, 4, 5 }, { 8, 8, 8, 8 } }; break;
                case 1: shape = new int[,] { { 2, 3, 2, 3 }, { 8, 8, 9, 9 } }; break;
                case 2: shape = new int[,] { { 2, 3, 4, 4 }, { 8, 8, 8, 9 } }; break;
                case 3: shape = new int[,] { { 2, 3, 4, 4 }, { 8, 8, 8, 7 } }; break;
                case 4: shape = new int[,] { { 3, 3, 4, 4 }, { 7, 8, 8, 9 } }; break;
                case 5: shape = new int[,] { { 3, 3, 4, 4 }, { 9, 8, 8, 7 } }; break;
                case 6: shape = new int[,] { { 3, 4, 4, 4 }, { 8, 7, 8, 9 } }; break;
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e) => TickTimer.Interval = 250;
        public new void Move(int x, int y){
            for (int i = 0; i < 4; i++) { 
                shape[1, i] += x;
                shape[0, i] += y;
            }
            if (FindMistake()) { 
                Move(-x, -y);
                if (y != 0) {
                    for (int i = 0; i < 4; i++)
                        field[shape[1, i], shape[0, i]]++;
                    SetShape(); 
                }
            }
            FillField();
        }
        public bool FindMistake(){
            for (int i = 0; i < 4; i++)
                if (shape[1, i] >= width || shape[0, i] >= height || shape[1, i] <= 0 || shape[0, i] <= 0 || field[shape[1, i], shape[0, i]] == 1)
                    return true; 
            return false;
        }
    }
}
/*Однажды мне в голову пришла идея - написать игру с минимальным функционалом в минимальное количество строк. Мой выбор игры пал на тетрис. В этой статье я опишу свой вариант тетриса.

Для начала стоит отметить, что в свою реализацию я включил только базовые возможности:
<ul>
	<li>Движение фигурок влево/вправо;</li>
	<li>Падение фигурок; </li>
	<li>Поворот фигурок; </li>
	<li>Удаление заполненных фигурок;</li>
	<li>Окончание игры. </li>
</ul>

Итак, сперва добавим на форму PictureBox и создадим таймер.
Также, для игры понадобятся:
<source lang="cs">
        public const int width = 15, height = 25, k = 15; // Размеры поля и размер клетки в пикселях
        public int[,] shape = new int[2, 4]; // Массив для хранения падающей фигурки (для каждого блока 2 координаты [0, i] и [1, i]
        public int[,] field = new int[width, height]; // Массив для хранения поля
        public Bitmap bitfield = new Bitmap(k * (width + 1) + 1, k * (height + 3) + 1);
        public Graphics gr; // Для рисования поля на PictureBox
</source>

Заполним поле по краям:
<source lang="cs">
            for (int i = 0; i < width; i++)
                field[i, height - 1] = 1;
            for (int i = 0; i < height; i++) {
                field[0, i] = 1;
                field[width - 1, i] = 1;
            }
</source>

Заполним фигуру:
<source lang="cs">
       public void SetShape(){
            Random x = new Random(DateTime.Now.Millisecond);
            switch (x.Next(7)){ // Рандомно выбираем 1 из 7 возможных фигурок
                case 0: shape = new int[,] { { 2, 3, 4, 5 }, { 8, 8, 8, 8 } }; break; 
                case 1: shape = new int[,] { { 2, 3, 2, 3 }, { 8, 8, 9, 9 } }; break;
                case 2: shape = new int[,] { { 2, 3, 4, 4 }, { 8, 8, 8, 9 } }; break;
                case 3: shape = new int[,] { { 2, 3, 4, 4 }, { 8, 8, 8, 7 } }; break;
                case 4: shape = new int[,] { { 3, 3, 4, 4 }, { 7, 8, 8, 9 } }; break;
                case 5: shape = new int[,] { { 3, 3, 4, 4 }, { 9, 8, 8, 7 } }; break;
                case 6: shape = new int[,] { { 3, 4, 4, 4 }, { 8, 7, 8, 9 } }; break;
            }
        }
</source>

Процедура, которая рисует "стакан" в PictureBox:
<source lang="cs">
public void FillField(){
            gr.Clear(Color.Black); //Очистим поле
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    if (field[i, j] == 1){ // Если клетка поля существует
                        gr.FillRectangle(Brushes.Green, i * k, j * k, k, k); // Рисуем в этом месте квадратик
                        gr.DrawRectangle(Pens.Black, i * k, j * k, k, k);
            }
            for (int i = 0; i < 4; i++){ // Рисуем падающую фигуру
                gr.FillRectangle(Brushes.Red, shape[1, i] * k, shape[0, i] * k, k, k);
                gr.DrawRectangle(Pens.Black, shape[1, i] * k, shape[0, i] * k, k, k);
            }
            FieldPictureBox.Image = bitfield;
        }
</source>

Фигурки я сначала двигаю, а потом проверяю, возможен такой вариант или нет. Если в каком-то месте находится ошибка (фигура вышла за пределы поля или наложилась на уже лежащую в поле фигуру), фигурка возвращается на своё прежнее место. Для этого я написал функцию, которая возвращает true, если на поле нашлась ошибка или false, если ошибки нет:
<source lang="cs">
       public bool FindMistake(){
            for (int i = 0; i < 4; i++)
                if (shape[1, i] >= width || shape[0, i] >= height || 
                    shape[1, i] <= 0 || shape[0, i] <= 0 || 
                    field[shape[1, i], shape[0, i]] == 1)
                        return true; 
            return false;
        }
</source>

Теперь приступим к передвижению фигурки. Создадим в конструкторе событие KeyDown.
Движение влево:
<source lang="cs">
      switch (e.KeyCode){
            case Keys.A:
                for (int i = 0; i < 4; i++) 
                    shape[1, i]--; // Сначала сдвигаем координаты всех кусочков фигуры на 1 влево по оси OX 
                if (FindMistake()) // Если после этого нашлась ошибка
                    for (int i = 0; i < 4; i++) 
                            shape[1, i]++; // Возвращаем фигурку обратно на 1 вправо
                break;
            ...
           }
</source>

Аналогично движение вправо:
<source lang="cs">
 case Keys.D:
     for (int i = 0; i < 4; i++) 
          shape[1, i]++;
     if (FindMistake())
          for (int i = 0; i < 4; i++)
               shape[1, i]--;
break;
</source>
*/