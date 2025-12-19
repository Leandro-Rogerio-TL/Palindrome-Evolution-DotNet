# High-Performance Longest Palindromic Substring

A C# implementation focusing on memory efficiency and computational performance. This project compares standard string manipulation against a zero-allocation approach using modern .NET features.

## 🚀 Performance Strategy

The optimized solution implements a **Center Expansion Algorithm** with the following engineering constraints:

- **Zero Heap Allocation**: Uses `ReadOnlySpan<char>` for string operations, ensuring no temporary strings are created during the search.
- **Early Exit (Pruning)**: The search engine calculates the maximum possible remaining palindrome length. If the remaining radius is smaller than the current record, the branch is pruned.
- **CPU Cache Locality**: Optimized memory access patterns to minimize cache misses.

## 📊 Benchmark Analysis (Final Results)

Tests conducted using **BenchmarkDotNet** on a complex alphanumeric dataset.

| Method | Mean | StdDev | Allocated | Alloc Ratio |
| :--- | :--- | :--- | :--- | :--- |
| **Span + Pruning (Dec/2025)** | **1.077 us** | 0.150 us | **88 B** | **0.04** |
| Legacy Substring (Feb/2025) | 15.890 us | 1.558 us | 2,384 B | 1.00 |

### Technical Insight
The optimized version proved to be **~15x faster** and **27x more memory-efficient**. While the legacy version generates significant Heap fragmentation (2,384 B per call), the new implementation allocates only the final result string (88 B), making it ideal for high-throughput systems and Big Data pipelines.

## 🛠 Implementation Details
- **Target Framework**: .NET 8.0
- **Key Technologies**: `ReadOnlySpan<char>`, BenchmarkDotNet.
