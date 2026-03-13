API_PROJECT_PATH = src/LXP.API/LXP.Api.csproj
SOLUTION_FILE = LXP.sln

# Build the solution
build:
	dotnet build $(SOLUTION_FILE)

# Run the API project
run: build
	dotnet run --project $(API_PROJECT_PATH)

# Clean the solution
clean:
	dotnet clean $(SOLUTION_FILE)

# Watch for changes and rebuild
watch:
	dotnet watch --project $(API_PROJECT_PATH) run

# Restore dependencies
restore:
	dotnet restore $(SOLUTION_FILE)

# Test the solution
test:
	dotnet test $(SOLUTION_FILE)

format:
	dotnet format $(SOLUTION_FILE)

# Publish the API project
publish:
	dotnet publish $(API_PROJECT_PATH) -o ./publish

# Serve documentation with DocFX
serve-docs:
	docfx D:\Backend\docfx.json --serve

# Default target
.PHONY: default build run clean watch restore test publish serve-docs
default: build run clean
