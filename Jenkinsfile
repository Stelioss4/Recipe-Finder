pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Restore') {
            steps {
                sh 'dotnet restore "Recipe Finder.sln"'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build "Recipe Finder.sln" --configuration Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                sh 'dotnet test "RecipeFinderTest/RecipeFinderTest.csproj" --configuration Release --no-build'
            }
        }
    }
}