using System;
using System.Text;
namespace BacterialForagingOptimization
{
    class BacterialForagingOptimizationProgram
    {
        static Random random = null;

        static void ShowVector(Double[] vector)
        {
            Console.Write("[");
            foreach (var item in vector)
            {
                Console.Write(item.ToString("F4")+" ");
            }
            Console.Write("]");
        }

        static void Main(string[] args)
        {
            try
            {
                int dim = 2;
                double minValue = -5.12;
                double maxValue = 5.12;
                int S = 100;
                int Nc = 20;
                int Ns = 5;
                int Nre = 8;
                int Ned = 4;
                double Ped = 0.25;
                double Ci = 0.05;
                random = new Random(0);
                Console.WriteLine("\nInitializing bacteria colony");
                Colony colony = new Colony(S, dim, minValue, maxValue);
                   for (int i = 0; i < S; ++i) {
                      double cost = Cost(colony.bacteria[i].position);
                      colony.bacteria[i].cost = cost;
                      colony.bacteria[i].prevCost = cost;
                   }

                double bestCost = colony.bacteria[0].cost;
                int indexOfBest = 0;
                for (int i = 0; i < S; ++i) {
                  if (colony.bacteria[i].cost < bestCost) {
                    bestCost = colony.bacteria[i].cost;
                    indexOfBest = i;
                  }
                }
                double[] bestPosition = new double[dim];
                colony.bacteria[indexOfBest].position.CopyTo(bestPosition, 0);
                Console.WriteLine("\nBest initial cost = " + bestCost.ToString("F4"));

                Console.WriteLine("\nEntering main BFO algorithm loop\n");
                int t = 0;
                for (int l = 0; l < Ned; ++l) // Eliminate-disperse loop
                {
                    for (int k = 0; k < Nre; ++k) // Reproduce-eliminate loop
                    {
                        for (int j = 0; j < Nc; ++j) // Chemotactic loop
                        {
                            // Reset the health of each bacterium to 0.0
                            for (int i = 0; i < S; ++i) { colony.bacteria[i].health = 0.0; }
                            for (int i = 0; i < S; ++i)
                            {
                                // Process each bacterium
                                double[] tumble = new double[dim];
                                for (int p = 0; p < dim; ++p) {
                                  tumble[p] = 2.0 * random.NextDouble() - 1.0;
                                }
                                double rootProduct = 0.0;
                                for (int p = 0; p < dim; ++p) {
                                  rootProduct += (tumble[p] * tumble[p]);
                                }
                                for (int p = 0; p < dim; ++p) {
                                  colony.bacteria[i].position[p] += (Ci * tumble[p]) / rootProduct;
                                }

                                colony.bacteria[i].prevCost = colony.bacteria[i].cost;
                                colony.bacteria[i].cost = Cost(colony.bacteria[i].position);
                                colony.bacteria[i].health += colony.bacteria[i].cost;
                                if (colony.bacteria[i].cost < bestCost) {
                                  Console.WriteLine("New best solution found by bacteria " + i.ToString()
                                    + " at time = " + t);
                                  bestCost = colony.bacteria[i].cost;
                                  colony.bacteria[i].position.CopyTo(bestPosition, 0);
                                }

                                int m = 0;
                                while (m < Ns && colony.bacteria[i].cost < colony.bacteria[i].prevCost) {
                                  ++m;
                                  for (int p = 0; p < dim; ++p) {
                                    colony.bacteria[i].position[p] += (Ci * tumble[p]) / rootProduct;
                                  }
                                  colony.bacteria[i].prevCost = colony.bacteria[i].cost;
                                  colony.bacteria[i].cost = Cost(colony.bacteria[i].position);
                                  if (colony.bacteria[i].cost < bestCost) {
                                    Console.WriteLine("New best solution found by bacteria " +
                                      i.ToString() + " at time = " + t);
                                    bestCost = colony.bacteria[i].cost;
                                    colony.bacteria[i].position.CopyTo(bestPosition, 0);
                                  }
                                } // while improving
                            }//i, each bacterium
                            ++t;// increment the time counter
                        }
                        // Reproduce healthiest bacteria, eliminate other half
                        Array.Sort(colony.bacteria);
                        for (int left = 0; left < S / 2; ++left) {
                        int right = left + S / 2;
                        colony.bacteria[left].position.CopyTo(colony.bacteria[right].position, 0);
                        colony.bacteria[right].cost = colony.bacteria[left].cost;
                        colony.bacteria[right].prevCost = colony.bacteria[left].prevCost;
                        colony.bacteria[right].health = colony.bacteria[left].health;
                      }
                    }// k, reproduction loop
                    // Eliminate-disperse
                    for (int i = 0; i < S; ++i) {
                    double prob = random.NextDouble();
                    if (prob < Ped) {
                      for (int p = 0; p < dim; ++p) {
                        double x = (maxValue - minValue) *
                          random.NextDouble() + minValue;
                        colony.bacteria[i].position[p] = x;
                      }
                      double cost = Cost(colony.bacteria[i].position);
                      colony.bacteria[i].cost = cost;
                      colony.bacteria[i].prevCost = cost;
                      colony.bacteria[i].health = 0.0;
                      if (colony.bacteria[i].cost < bestCost) {
                        Console.WriteLine("New best solution found by bacteria " +
                          i.ToString() + " at time = " + t);
                        bestCost = colony.bacteria[i].cost;
                        colony.bacteria[i].position.CopyTo(bestPosition, 0);
                      }
                    } // if (prob < Ped)
                  } // for
                }//l, elimination-dispersal loop
                Console.WriteLine("\n\nAll BFO processing complete");
                Console.WriteLine("\nBest cost (minimum function value) found = " +
                  bestCost.ToString("F4"));
                Console.Write("Best position/solution = ");
                ShowVector(bestPosition);
                Console.WriteLine("\nEnd BFO demo\n");
                Console.ReadLine();
              }
              catch (Exception ex)
              {
                Console.WriteLine("Fatal: " + ex.Message);
                Console.ReadLine();
              }
        } // Main
        static double Cost(double[] position) {
        double result = 0.0;
        for (int i = 0; i < position.Length; ++i) {
          double xi = position[i];
          result += (xi * xi) - (10 * Math.Cos(2 * Math.PI * xi)) + 10;
        }
        return result;
        }
    }
      
   

    public class Colony // Collection of Bacterium
    {
        public Bacterium[] bacteria;
        public Colony(int S, int dim, double minValue, double maxValue)
        {
            this.bacteria = new Bacterium[S];
            for (int i = 0; i < S; ++i)
                bacteria[i] = new Bacterium(dim, minValue, maxValue);
        }
        public override string ToString() {
            String s = "";
           // List<String> sb = new List<String>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bacteria.Length; i++)
            {
                sb.Append("[").Append(i).Append("]:").Append(bacteria[i].ToString()).Append(Environment.NewLine);
                s += sb.ToString();
            }
            return s;
        }
        public class Bacterium : IComparable<Bacterium>
        {
            public double[] position;
            public double cost;
            public double prevCost;
            public double health;
            static Random random = new Random(0);
            public Bacterium(int dim, double minValue, double maxValue)
            {
                this.position = new double[dim];
                for (int p = 0; p < dim; ++p)
                {
                    double x = (maxValue - minValue) * random.NextDouble() + minValue;
                    this.position[p] = x;
                }
                this.health = 0.0;
            }
            public override string ToString()
            {
                string s = "[";
                StringBuilder sb = new StringBuilder();
                Double[] pos = new Double[position.Length];
                for (int i = 0; i < position.Length; i++)
                {
                    pos[i] = position[i];
                }
               
                for (int p = 0; p < position.Length - 1; p++)
                {
                    sb.Append(pos[p].ToString("F2")).Append(" ");
                    s += sb.ToString();
                }

                s += "]";
                s += "cost = " + cost.ToString("F2") + " prevCost = " + prevCost.ToString("F2");
                s += " health = " + health.ToString("F2");

                return s;
            }
            public int CompareTo(Bacterium other)
            {
                if (this.health < other.health)
                    return -1;
                else if (this.health > other.health)
                    return 1;
                else
                    return 0;
            }
        }

    }
} // ns



