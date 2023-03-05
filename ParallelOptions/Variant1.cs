using System;
using System.Collections.Generic;
using System.Threading;

namespace ParallelOptions;

using System.Threading.Tasks;

public class Variant1
{
    private readonly List<int> _list1 = new();
    private readonly List<int> _list2 = new();
    private readonly List<int> _list3 = new();

    public async Task Run()
    {
        Console.WriteLine("Starting...");
        FillListWithValues(_list1);
        FillListWithValues(_list2);
        FillListWithValues(_list3);

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 3
        };

        var parallelRuns = new List<Task>();
        parallelRuns.Add(Task.Run(() => RunList(_list1, parallelOptions, 1)));
        parallelRuns.Add(Task.Run(() => RunList(_list2, parallelOptions, 2)));
        parallelRuns.Add(Task.Run(() => RunList(_list3, parallelOptions, 3)));
        parallelRuns.Add(Task.Run(() =>
        {
            for (var i = 0; i <= Constants.MaxListValue; i++)
            {
                Console.WriteLine("---------------------------");
                Thread.Sleep(TimeSpan.FromMilliseconds(Constants.WaitTimeSpanValue));
            }
        }));


        await Task.WhenAll(parallelRuns);
    }

    void RunList(IList<int> list, ParallelOptions parallelOptions, int listNo)
    {
        Parallel.ForEach(list, parallelOptions, (index) => DisplaySomething(listNo, index));
    }

    static void FillListWithValues(ICollection<int> list)
    {
        for (var i = 0; i <= Constants.MaxListValue; i++)
        {
            list.Add(i);
        }
    }

    void DisplaySomething(int listNo, int index)
    {
        Console.WriteLine($"{DateTime.Now} List no: {listNo}, Thread no: {index}");
        Thread.Sleep(TimeSpan.FromMilliseconds(Constants.WaitTimeSpanValue));
    }
}