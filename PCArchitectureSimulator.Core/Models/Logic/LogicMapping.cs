// Use of 'Span' allows for efficient and safe memory usage while avoiding unnecessary allocations
public delegate void LogicMapping(ReadOnlySpan<LogicState> Inputs, Span<LogicState> Outputs);