using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Cell
    {
        public Cell()
        {

        }

        public Cell(Cell c)//copy constructor
        {
            solved = c.solved;
            for (int i = 0; i < 9; ++i)
            {
                intersectingNumbers[i] = c.intersectingNumbers[i];
            }
        }

        public int solved = 0;//number this cell is 0 for unsolved, any number 1-9 solved
        public bool[] intersectingNumbers = { false, false, false, false, false, false, false, false, false};//numbers that are intersecting this cell

        public void union(bool[] b)//combine two intersecting numbers
        {
            for (int i = 0; i < 9; ++i)
            {
                if (b[i])
                    intersectingNumbers[i] = b[i];
            }
        }

        //returns true if it was solved by this call it will be false if the number is solved previous to the call
        public bool trySolvingByElimination()
        {
            if (intersectingNumbers.Where(c => !c).Count() == 1 && solved == 0)//there is only one number that is false in solvedNumbers and the cell is not already solved
            {
                //test output
                /*Console.Write("solved throught elemination " + x + "," + y + " ");
                for (int i = 0; i < 9; ++i)
                    Console.Write(Convert.ToInt32(solvedNumbers[i]) + " ");
                Console.Write('\n');//*/
                solved = Array.IndexOf(intersectingNumbers, false) + 1;
                return true;//cell changed
            }
            return false;//cell did not change
        }

        public List<int> getNonIntersectingNumbers()//get a list of the numbers that arent intersecting
        {
            List<int> pN = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                if (intersectingNumbers[i] == false)
                    pN.Add(i);
            }
            return pN;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class SudokuBoard
    {
        private Cell[,] board = new Cell[9,9];//the board

        
        public SudokuBoard(Cell[,] board)//constructor
        {
            this.board = board;
        }

        public void ChangeBoard(Cell[,] board)//changes the board to solved
        {
            this.board = board;
        }

        public bool solve()//solve the current board
        {
            bool solved = false;
            bool changesMadeToBoard = false;

            Section[] sectionSolverArray = new Section[3];//contain the three types of section solvers
            sectionSolverArray[0] = new Row(board);
            sectionSolverArray[1] = new Column(board);
            sectionSolverArray[2] = new Block(board);

            List<int>[] sectionsUnsolved = new List<int>[3];//list that will hold the rows, columns, and blocks to check
            //intitilize all the lists
            sectionsUnsolved[0] = new List<int>();
            sectionsUnsolved[1] = new List<int>();
            sectionsUnsolved[2] = new List<int>();

            for (int i = 0; i < 9; ++i)//add the number 0-8 to the list
            {
                sectionsUnsolved[0].Add(i);
                sectionsUnsolved[1].Add(i);
                sectionsUnsolved[2].Add(i);
            }

            while (!solved)//continue till solved
            {
                //if experiencing errors try reseting interscting numbers for all cells(code commened out underneath)
                /*                
                for (int x = 0; x < 9; ++x)//go though the x of the block
                {
                    for (int y = 0; y < 9; ++y)//go throught the y of the block
                    {
                        board[x, y].intersectingNumbers = new bool[9];
                        for (int t = 0; t < 9; ++t)//go throught the y of the block
                        {
                            board[x, y].intersectingNumbers[t] = false;
                        }
                    }
                }*/

                //check all the boards row, columns, and blocks and updating the intersecting numbers of all cells
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < sectionsUnsolved[i].Count(); ++j)
                    {
                        if (sectionSolverArray[i].updateIntersection(sectionsUnsolved[i][j]))//if the current row, column or block is solved
                        {
                            sectionsUnsolved[i].RemoveAt(j);//remove it from the list
                            j--;//account for it by moving one position back
                        }
                    }
                }

                solved = true;
                changesMadeToBoard = false;
                //solve throught elemination
                for (int x = 0; x < 9; ++x)
                    for (int y = 0; y < 9; ++y)
                    {
                        if (board[x, y].trySolvingByElimination())//if position is solved during this round of testing(aka something was changed)
                            changesMadeToBoard = true;//board was changed
                        if (board[x, y].solved == 0)//if a position is unsolved
                            solved = false;//board is not solved
                    }

                //if experiencing errors try reset all intersecting numbers for each cell

                //check all the boards row, columns, and blocks and updating the intersecting numbers of all cells
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < sectionsUnsolved[i].Count(); ++j)
                    {
                        if (sectionSolverArray[i].updateIntersection(sectionsUnsolved[i][j]))//if the current row, column or block is solved
                        {
                            sectionsUnsolved[i].RemoveAt(j);//remove it from the list
                            j--;//account for it by moving one position back
                        }
                    }
                }

                //solve by reasoning
                //check to see if each block has only one position availible for a number
                int counter = 0;
                int xPos = 0, yPos = 0;
                for (int block = 0; block < 9; ++block)//go throught all the blocks
                    for (int i = 0; i < 9; ++i)//i represents the possible number to be checked
                    {
                        for (int x = 0; x < 3; ++x)//go though the x of the block
                        {
                            for (int y = 0; y < 3; ++y)//go throught the y of the block
                            {
                                if (board[(block / 3) * 3 + x, (block % 3) * 3 + y].intersectingNumbers[i] == false && board[(block / 3) * 3 + x, (block % 3) * 3 + y].solved == 0)// && numbersSolved[i] == false) //the number i was already in this block
                                {
                                    counter++;//count all the false
                                    //record the position
                                    xPos = x;
                                    yPos = y;
                                }
                            }
                        }
                        if (counter == 1)//if there was only one false
                        {
                            //a test print statment
                            /*Console.Write("solved throught reasoning y" + ((block%3) * 3 + yPos) + ", x" + ((block / 3) * 3 + xPos) + " " + (i + 1) + "\n");
                            for (int x = 0; x < 3; ++x)
                            {
                                for (int y = 0; y < 3; ++y)
                                {
                                    if (board[blockX * 3 + x, blockY * 3 + y].solved == 0)
                                        Console.Write(Convert.ToInt32(board[blockX * 3 + x, blockY * 3 + y].solvedNumbers[i]) + " ");
                                }
                            }
                            Console.Write('\n');//*/
                            changesMadeToBoard = true;
                            board[(block / 3) * 3 + xPos, (block % 3) * 3 + yPos].solved = i + 1;
                            solved = false;
                        }
                        counter = 0;
                    }

                if (!changesMadeToBoard && !solved)//changes made to board=false and solved=false (if the board is not changed and not solved)
                {
                    //Console.Write("board not changed\n/////////////////\n");
                    //printBoard();
                    int guess = findBestGuess();
                    if (guess < 0)
                    {
                        Console.WriteLine(guess);
                        return false;
                    }
                    //Console.Write("guess position: " + guess + "\n");

                    List<int> pN = board[(guess / 9), (guess % 9)].getNonIntersectingNumbers();//get the list of all non intersecting numbers of the best guess
                    for (int i = 0; i < pN.Count(); ++i)
                    {
                        Cell[,] b = new Cell[9, 9];
                        for (int x = 0; x < 9; ++x)//go though the x of the block
                        {
                            for (int y = 0; y < 9; ++y)//go throught the y of the block
                            {
                                b[x, y] = new Cell(board[x, y]);
                            }
                        }
                        //Console.Write("making guess of " + (pN[i] + 1) + " i:" + i + " x,y: " + ((guess / 9)) + ", " + ((guess % 9)) + "\n" + "\n");
                        b[(guess / 9), (guess % 9)].solved = pN[i] + 1;
                        SudokuBoard s = new SudokuBoard(b);
                        if (s.solve())//try to solve for this board if it cant then it will return to here(true or false) continueing onto the next permutation
                            return true;//will continue to return true untill it reaches the original permutation it started to make guesses at

                        //reset intersecting numbers
                    }
                    //Console.WriteLine("revert back to a board guess");
                    return false;
                }
                //reset intersecting numbers

            }
            //Console.Write("checking board if solved\n");
            bool checkBoardSolved = checkIfSolved();
            if (checkBoardSolved)
            {
                printBoard();
            }
            //return if true if correctly solved false if not correctly solved
            return checkBoardSolved;
        }

        public bool checkIfSolved()//checks to see if all rows and columns have uniqe numbers(WARNING doesnt not work for every possible case should find a better method)
        {
            for (int i = 0; i < 9; ++i)//go through and a row and column
            {
                bool[] rowUniuqeNumbers = new bool[9];
                bool[] columnUniqueNumbers = new bool[9];
                for (int x = 0; x < 9; ++x)//go through the rows and columns checking adding the numbers by index that are unqiue
                {
                    rowUniuqeNumbers[board[i, x].solved - 1] = true;
                    columnUniqueNumbers[board[x, i].solved - 1] = true;
                }

                int row = 0, column = 0;
                for (int y = 0; y < 9; ++y)//
                {
                    row += Convert.ToInt16(rowUniuqeNumbers[y]);
                    column += Convert.ToInt16(columnUniqueNumbers[y]);
                }
                if (row != 9 || column != 9)//if both of them dont have all nine numbers
                {
                    return false;
                }

            }
            Console.Write("Board Solved\n");
            return true;
        }

        public int findBestGuess()//finds best guess based on intersecting numbers could find a better guess(possibly) if you use the reasoning method but it is not too bad(just saying there is probably a better method)
        {
            int xPos = -1, yPos = -1, currentBestGuess = -1;
            for (int x = 0; x < 9; ++x)
                for (int y = 0; y < 9; ++y)//go throught the entire board
                {
                    if (board[x, y].solved == 0)//if unsolved
                    {
                        int counter = 0;
                        for (int i = 0; i < 9; ++i)//adds all intersecting numbers together
                        {
                            counter += Convert.ToInt32(board[x, y].intersectingNumbers[i]);
                        }
                        if (counter > currentBestGuess)//if the ammount of intersecting numbers greater then the current guess
                        {
                            //mark this position as the new best guess
                            currentBestGuess = counter;
                            xPos = x;
                            yPos = y;
                        }
                    }
                }
            //return the position as a 1d positional array x = num/9 y = num % 9
            return xPos * 9 + yPos;
        }

        public void printBoard()//prints the board with lines so its easier to read
        {
            Console.Write("print board\n");
            for (int x = 0; x < 9; ++x)
            {
                for (int y = 0; y < 9; ++y)
                {
                    Console.Write(board[x, y].solved + " ");
                    if (y == 2 || y == 5)
                        Console.Write("|");
                }
                Console.Write("\n");
                for (int y = 0; y < 19 && (x == 2 || x == 5); ++y)
                {
                    Console.Write("-");
                    if (y == 18)
                        Console.Write('\n');
                }
            }
            Console.WriteLine("\n");
            //.ReadKey();
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 


    public abstract class Section
    {
        abstract public Cell[,] board {
            get;         
        }
        abstract public bool updateIntersection(int sectionNumber);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    public class Row : Section
    {
        public override Cell[,] board { get; }
        public Row(Cell[,] board)
        {
            this.board = board;
        }

        public override bool updateIntersection(int row)
        {
            bool[] numbersSolved = new bool[9];
            for (int i = 0; i < 9; ++i)
            {
                if (board[i, row].solved != 0)//if the cell is solved
                {
                    numbersSolved[board[i, row].solved - 1] = true;//convetes the solved numbers to the array bounds
                    continue;
                }

            }
            for (int i = 0; i < 9; ++i)
            {
                if(board[i, row].solved == 0)//the cell is not solved
                {
                    board[i, row].union(numbersSolved);//union the arrays to gether(add all the trues together to keep track of all the number solved for all row/colums/blocks that contain this cell)
                }
            }

            return numbersSolved.All(x => x);//true if all numbers are solved true if one number is false
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    public class Column : Section
    {
        public override Cell[,] board { get; }
        public Column(Cell[,] board)
        {
            this.board = board;
        }
        public override bool updateIntersection(int column)
        {
            bool[] numbersSolved = new bool[9];
            for (int i = 0; i < 9; ++i)
            {
                if (board[column, i].solved != 0)//if the cell is solved
                {
                    numbersSolved[board[column, i].solved - 1] = true;//convetes the solved numbers to the array bounds
                    continue;
                }

            }
            for (int i = 0; i < 9; ++i)
            {
                if(board[column, i].solved == 0)//the cell is not solved
                {
                    board[column, i].union(numbersSolved);//union the arrays to gether(add all the trues together to keep track of all the number solved for all row/colums/blocks that contain this cell)
                }
            }

            return numbersSolved.All(x => x);//true if all numbers are solved true if one number is false
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    public class Block : Section
    {
        public override Cell[,] board { get; }
        public Block(Cell[,] board)
        {
            this.board = board;
        }
        public override bool updateIntersection(int block)//0-8
        {
            int blockX = block / 3, blockY = block % 3;
            bool[] numbersSolved = new bool[9];
            //go throught the 3x3 grid for the solved numbers
            for (int x = 0; x < 3; ++x)
            {
                for (int y = 0; y < 3; ++y)
                {
                    if (board[blockX * 3 + x, blockY * 3 + y].solved != 0)//if the cell is solved
                    {
                        numbersSolved[board[(blockX * 3) + x, (blockY * 3) + y].solved - 1] = true;//convetes the solved numbers to the array bounds
                        continue;
                    }
                }
            }

            for (int x = 0; x < 3; ++x)
            {
                for (int y = 0; y < 3; ++y)
                {
                    if(board[(blockX * 3) + x, (blockY * 3) + y].solved == 0)//the cell is not solved
                    {
                        board[(blockX * 3) + x, (blockY * 3) + y].union(numbersSolved);//union the arrays together(add all the trues together to keep track of all the number solved for all row/colums/blocks that contain this cell)
                    }
                }
            }
            return numbersSolved.All(x => x);//true if all numbers are solved true if one number is false
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    class Program
    {
        static void Main(string[] args)
        {
            Cell[,] c = new Cell[9,9];
            for (int x = 0; x < 9; ++x)
                for (int y = 0; y < 9; ++y)
                {
                    c[x, y] = new Cell();
                }

            //test the block resoning solver
            /*c[0, 3].solved = 1;
            c[1, 1].solved = 1;
            c[3, 4].solved = 1;
            c[4, 0].solved = 1;
            c[5, 8].solved = 1;
            c[6, 6].solved = 1;
            c[7, 5].solved = 1;
            c[8, 2].solved = 1;*/

            //test case 1
            /*c[0, 0].solved = 5;
            c[0, 1].solved = 3;
            c[0, 4].solved = 7;

            c[1, 0].solved = 6;
            c[1, 3].solved = 1;
            c[1, 4].solved = 9;
            c[1, 5].solved = 5;

            c[2, 1].solved = 9;
            c[2, 2].solved = 8;
            c[2, 7].solved = 6;

            c[3, 0].solved = 8;
            c[3, 4].solved = 6;
            c[3, 8].solved = 3;

            c[4, 0].solved = 4;
            c[4, 3].solved = 8;
            c[4, 5].solved = 3;
            c[4, 8].solved = 1;

            c[5, 0].solved = 7;
            c[5, 4].solved = 2;
            c[5, 8].solved = 6;

            c[6, 1].solved = 6;
            c[6, 6].solved = 2;
            c[6, 7].solved = 8;

            c[7, 3].solved = 4;
            c[7, 4].solved = 1;
            c[7, 5].solved = 9;
            c[7, 8].solved = 5;

            c[8, 4].solved = 8;
            c[8, 7].solved = 7;
            c[8, 8].solved = 9;//*/

            //test case 3 not logically solvable
            /*c[1, 5].solved = 3;
            c[1, 7].solved = 8;
            c[1, 8].solved = 5;

            c[2, 2].solved = 1;
            c[2, 4].solved = 2;

            c[3, 3].solved = 5;
            c[3, 5].solved = 7;

            c[4, 2].solved = 4;
            c[4, 6].solved = 1;

            c[5, 1].solved = 9;

            c[6, 0].solved = 5;
            c[6, 7].solved = 7;
            c[6, 8].solved = 3;

            c[7, 2].solved = 2;
            c[7, 4].solved = 1;

            c[8, 4].solved = 4;
            c[8, 8].solved = 9;//*/

            //test case 4 isollate bug when trying to recusivly solve
            /*c[0, 7].solved = 2;
            c[0, 8].solved = 1;

            c[1, 0].solved = 2;
            c[1, 1].solved = 4;
            c[1, 2].solved = 6;
            c[1, 3].solved = 1;
            c[1, 4].solved = 7;
            c[1, 5].solved = 3;
            c[1, 6].solved = 9;
            c[1, 7].solved = 8;
            c[1, 8].solved = 5;

            c[2, 2].solved = 1;
            c[2, 4].solved = 2;

            c[3, 0].solved = 1;
            c[3, 2].solved = 8;
            c[3, 3].solved = 5;
            c[3, 5].solved = 7;
            c[3, 6].solved = 6;

            c[4, 2].solved = 4;
            c[4, 6].solved = 1;

            c[5, 1].solved = 9;
            c[5, 3].solved = 4;
            c[5, 5].solved = 1;

            c[6, 0].solved = 5;
            c[6, 1].solved = 1;
            c[6, 2].solved = 9;
            c[6, 6].solved = 4;
            c[6, 7].solved = 7;
            c[6, 8].solved = 3;

            c[7, 0].solved = 4;
            c[7, 2].solved = 2;
            c[7, 4].solved = 1;

            c[8, 4].solved = 4;
            c[8, 6].solved = 2;
            c[8, 7].solved = 1;
            c[8, 8].solved = 9;//*/

            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Brandon\Documents\sudokuBoard.txt");

            for (int x = 0; x < lines.Count(); ++x)
            {
                for (int y = 0; y < lines[x].Length; ++y)
                {
                    c[x, y].solved = lines[x][y] - '0';
                }
            }

            SudokuBoard b = new SudokuBoard(c);
            b.printBoard();
            b.solve();
            //b.printBoard();
        }
    }
}
