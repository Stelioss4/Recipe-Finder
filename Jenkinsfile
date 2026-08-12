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

        stage('Docker Build') {
            steps {
                sh '''
                    /snap/docker/current/bin/docker build \
                    -f "RecipeFinder WebApp/Dockerfile" \
                    -t recipefinder-ci:${BUILD_NUMBER} \
                    .
                '''
            }
        }

        stage('Verify Docker Image') {
            steps {
                sh '/snap/docker/current/bin/docker image inspect recipefinder-ci:${BUILD_NUMBER}'
            }
        }
    }
}