# Install the tool
dotnet tool install --global dotnet-counters

# Run your API application
dotnet run --project YourApiProject

# In another terminal, find the process ID
dotnet-counters ps

# Monitor thread pool metrics
dotnet-counters monitor --process-id <PID> System.Runtime

# Look for these metrics:
# - ThreadPool Thread Count
# - ThreadPool Queue Length
# - ThreadPool Completed Work Item Count