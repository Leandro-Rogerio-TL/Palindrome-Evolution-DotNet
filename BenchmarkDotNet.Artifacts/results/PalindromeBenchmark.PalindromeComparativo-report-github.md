```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26100.7462/24H2/2024Update/HudsonValley)
12th Gen Intel Core i5-1235U 1.30GHz, 1 CPU, 12 logical and 10 physical cores
.NET SDK 8.0.404
  [Host]     : .NET 8.0.12 (8.0.12, 8.0.1224.60305), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.12 (8.0.12, 8.0.1224.60305), X64 RyuJIT x86-64-v3


```
| Method                              | Mean     | Error    | StdDev    | Median   | Ratio | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
|------------------------------------ |---------:|---------:|----------:|---------:|------:|--------:|-----:|-------:|----------:|------------:|
| &#39;Dez/2025: Otimizado (Span + Poda)&#39; | 71.83 ns | 3.541 ns | 10.441 ns | 67.06 ns |  0.97 |    0.16 |    1 |      - |         - |        0.00 |
| &#39;Fev/2025: Legado (Substring)&#39;      | 74.61 ns | 1.855 ns |  5.380 ns | 74.80 ns |  1.01 |    0.10 |    1 | 0.0038 |      24 B |        1.00 |
