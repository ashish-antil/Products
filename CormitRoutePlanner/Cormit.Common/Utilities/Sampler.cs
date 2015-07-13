//namespace Imarda.Lib
//{
//  public class Sampler
//  {
//    private readonly int _Factor;
//    private int _Max;
//    private int _Step;
//    private int _Previous;

//    /// <summary>
//    /// Create an object that gets fed an incrementing sequence: 1,2,3,4,5,.... and decides whether 
//    /// to use a number from the sequence or not. It does so by skipping progressively more numbers,
//    /// first going up to 'initialMax' and using 'initialStep' as sample step. When the max is reached,
//    /// a new max and step are calculated by multiplying with the factor.
//    /// E.g. intialMax=100, initialStep=1, factor=10:
//    /// 1, 2, 3...100, (!) 110, 120, 130...1000, (!) 1100, 1200, 1300..10000, (!) 11000, 12000... 100000
//    /// (!) = apply factor to max and step.
//    /// Typical application of this: error log printing the same error text over and over, filling up disk space with
//    /// useless info. By using this sampler the errors logged are reduced to Order(log n).
//    /// </summary>
//    /// <param name="initialMax">first time the factor will get applied</param>
//    /// <param name="initialStep">first sample step up until max</param>
//    /// <param name="factor">apply to max and step each time max is reached</param>
//    public Sampler(int initialMax, int initialStep, int factor)
//    {
//      _Max = initialMax;
//      _Step = initialStep;
//      _Factor = factor;
//    }

//    /// <summary>
//    /// Decide whether to use the number passed in (true) or to skip it (false).
//    /// Numbers must be increasing, a lesser number than a previous one will return false.
//    /// </summary>
//    /// <param name="n">number to test</param>
//    /// <returns>true if to be used, false if to be skipped by caller</returns>
//    public bool Use(int n)
//    {
//      if (n < _Previous) return false;
//      if (n > _Max)
//      {
//        _Max *= _Factor;
//        _Step *= _Factor;
//      }
//      _Previous = n;
//      return n % _Step == 0;
//    }
//  }
//}
