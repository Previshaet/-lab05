using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static ConsoleApplication4.Program;

namespace ConsoleApplication4
{
    class Program
    {
        //part 1

        public static int min = 0;
        public static int max = 9;

        class MyMatrix
        {
            internal int _m = 0;
            internal int _n = 0;
            internal int[][] _matrix;

            public MyMatrix(int m = 0, int n = 0)
            {
                _m = m;
                _n = n;
                Random random = new Random();
                _matrix = new int[m][];
                for (int i = 0; i < m; ++i)
                {
                    _matrix[i] = new int[n];
                    for (int j = 0; j < n; ++j)
                    {
                        Random r = new Random();
                        _matrix[i][j] = min + (r.Next()) % (max - min + 1);
                    }
                }
            }


            public void Fill()
            {
                for (int i = 0; i < _m; ++i)
                {
                    for (int j = 0; j < _n; ++j)
                    {
                        Random r = new Random();
                        _matrix[i][j] = r.Next(min, max);
                    }
                }
            }

            public void ShowPartialy(int i1, int j1, int i2, int j2)
            {
                for (int i = i1; i < i2+1; ++i)
                {
                    Console.Write('[');
                    for (int j = j1; j < j2+1; ++j)
                    {
                        Console.Write($"{_matrix[i][j]} ");
                    }
                    Console.WriteLine(']');
                }
            }

            public void Show()
            {
                if (_m == 0)
                {
                    Console.WriteLine("Empty.");
                }
                else
                {
                    for (int i = 0; i < _m; ++i)
                    {
                        Console.Write("[");
                        for (int j = 0; j < _n; ++j)
                        {
                            Console.Write($"{_matrix[i][j]} ");
                        }
                        Console.WriteLine("]");
                    }
                }
            }

            public void ChangeSize(int new_m=0, int new_n=0)
            {
                int[][] new_matrix = new int[new_m][];
                Random random = new Random();
                for (int i = 0; i < new_m; ++i)
                {
                    new_matrix[i] = new int[new_n];
                    for (int j = 0; j < new_n; ++j)
                    {
                        Random r = new Random();
                        if (i < _m && j < _n)
                        {
                            new_matrix[i][j] = _matrix[i][j];
                        }
                        else
                        {
                            new_matrix[i][j] = min + (r.Next()) % (max - min + 1);
                        }
                    }
                }
                (_matrix, new_matrix) = (new_matrix, _matrix);
            }

            public int this[int i, int j]
            {
                get => _matrix[i][j];

                set => _matrix[i][j] = value;
            }
        }

        //part 2
        public class MyList<T>
        {
            T[] _list;
            int _count;
            public MyList(T[] list)
            {
                _list = list;
                _count = list.Length;
            }

            public int Count => _count;

            public T this[int i]
            {
                get => _list[i];

                set => _list[i] = value;
            }

            public void Insert(T x, int index = 0)
            {
                if (index > _count)
                {
                    Console.WriteLine($"Error: index {index} is out of range.");
                }
                else
                {
                    T[] new_list = new T[_count+1];
                    for (int i = 0; i < index; ++i)
                    {
                        new_list[i] = _list[i];
                    }
                    new_list[index] = x;
                    for (int i = index+1; i < _count+1; ++i)
                    {
                        new_list[i] = _list[i-1];
                    }
                    (_list, new_list) = (new_list, _list);
                    _count++;
                }
            }

            public override string ToString()
            {
                string rez = "{";
                for (int i = 0; i < _count; ++i)
                {
                    rez += _list[i].ToString() + "; ";
                }
                rez += "}";
                return rez;
            }
        }


        //part 3
        public class MyDictionary<TKey, TValue>:IEnumerable<Tuple<TKey, TValue>>
        {
            Tuple<TKey, TValue>[] _dictionary = new Tuple<TKey, TValue>[0];
            int _count = 0;

            public int Count => _count;

            public Tuple<TKey, TValue> this[int i]
            {
                get => _dictionary[i];

                set => _dictionary[i] = value;
            }

            public void Insert(TKey key, TValue val)
            {
                Tuple<TKey, TValue> pair = new Tuple<TKey, TValue>(key, val);
                int i = 0;
                while (i < _count && !_dictionary[i].Item1.Equals(pair.Item1))
                {
                    i++;
                }
                if (i < Count)
                {
                    _dictionary[i] = pair;
                }
                else
                {
                    Tuple<TKey, TValue>[] new_dic = new Tuple<TKey, TValue>[_count + 1];
                    for (i = 0; i < _count; ++i)
                    {
                        new_dic[i] = _dictionary[i];
                    }
                    new_dic[_count] = pair;
                    _count++;
                    (_dictionary, new_dic) = (new_dic, _dictionary);
                }
            }

            public void Insert(Tuple<TKey, TValue> pair) 
            {
                int i = 0;
                while (i < _count && !_dictionary[i].Item1.Equals(pair.Item1))
                {
                    i++;
                }
                if (i < Count)
                {
                    _dictionary[i] = pair;
                }
                else
                {
                    Tuple<TKey, TValue>[] new_dic = new Tuple<TKey, TValue>[_count + 1];
                    for (i = 0; i < _count; ++i)
                    {
                        new_dic[i] = _dictionary[i];
                    }
                    new_dic[_count] = pair;
                    _count++;
                    (_dictionary, new_dic) = (new_dic, _dictionary);
                }
            }

            public IEnumerator<Tuple<TKey, TValue> > GetEnumerator()
            {
                for (int i = 0; i < _count; i++)
                {
                    yield return this[i];
                }
            }

            public void Add(Tuple<TKey, TValue> pair)
            {
                Tuple<TKey, TValue>[] new_dic = new Tuple<TKey, TValue>[_count + 1];
                for (int i = 0; i < _count; ++i)
                {
                    new_dic[i] = _dictionary[i];
                }
                new_dic[_count] = pair;
                _count++;
                (_dictionary, new_dic) = (new_dic, _dictionary);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        static void Main(string[] args)
        {
            //part 1
            Console.WriteLine("Part 1:");
            int n, m;
            Console.WriteLine("Enter m, n:");
            string input = Console.ReadLine(); //получает строку с консоли
            var granica = input.Split(' '); //разделить на два элемента по пробелу
            int.TryParse(granica[0], out m);
            int.TryParse(granica[1], out n);

            MyMatrix mat = new MyMatrix(m, n);
            Console.WriteLine("Matrix:");
            mat.Show();

            mat.Fill();
            Console.WriteLine("Re made Matrix:");
            mat.ShowPartialy(0, 0, m-1, n-1);

            Console.WriteLine("Enter new m, n:");
            input = Console.ReadLine(); //получает строку с консоли
            granica = input.Split(' '); //разделить на два элемента по пробелу
            int.TryParse(granica[0], out m);
            int.TryParse(granica[1], out n);

            mat.ChangeSize(m, n);
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    Console.Write($"{mat[i, j]} ");
                }
                Console.WriteLine(";");
            }

            //part 2
            Console.WriteLine("\nPart 2:");
            MyList<int> list = new MyList<int>(new int[] { 1, 2, 3, 4 });
            Console.WriteLine($"List: \n{list}");
            
            Console.WriteLine($"List length: \n{list.Count}");

            list[3] = 5;
            list.Insert(0);
            list.Insert(6, list.Count);
            Console.WriteLine($"Changed List: \n{list}");

            //part 3
            Console.WriteLine("\nPart 3:");

            MyDictionary<int, int> dic = new MyDictionary<int, int>() {new Tuple<int, int>(0,1) };
            dic.Insert(1, 2);
            dic.Insert(2, 50);
            dic.Insert(3, 696);


            Console.WriteLine($"Dictionary: ");
            foreach (Tuple<int,int> pair in dic)
            {
                Console.WriteLine($"  {{{pair.Item1} : {pair.Item2}}}");
            }
            Console.WriteLine($"Count:  {dic.Count}");

            dic.Insert(1, 2222);
            dic.Insert(99, 80082);
            Console.WriteLine($"Changed Dictionary: ");
            for (int i = 0; i < dic.Count; ++i)
            {
                Console.WriteLine($"  {{{dic[i].Item1} : {dic[i].Item2}}}");
            }
            Console.Read();
        }
    }
}
