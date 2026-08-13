pipeline {
    agent any

    environment {
        DOCKER_IMAGE = 'steliosboursanidis/recipefinderwebapp'
        DOCKER_BIN = '/snap/docker/current/bin/docker'
    }

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
                    $DOCKER_BIN build \
                    -f "RecipeFinder WebApp/Dockerfile" \
                    -t $DOCKER_IMAGE:${BUILD_NUMBER} \
                    -t $DOCKER_IMAGE:latest \
                    .
                '''
            }
        }

        stage('Verify Docker Image') {
            steps {
                sh '$DOCKER_BIN image inspect $DOCKER_IMAGE:${BUILD_NUMBER}'
            }
        }

        stage('Docker Push') {
            steps {
                withCredentials([
                    usernamePassword(
                        credentialsId: 'dockerhub-credentials',
                        usernameVariable: 'DOCKER_USERNAME',
                        passwordVariable: 'DOCKER_TOKEN'
                    )
                ]) {
                    sh '''
                        echo "$DOCKER_TOKEN" | $DOCKER_BIN login \
                            --username "$DOCKER_USERNAME" \
                            --password-stdin

                        $DOCKER_BIN push $DOCKER_IMAGE:${BUILD_NUMBER}
                        $DOCKER_BIN push $DOCKER_IMAGE:latest

                        $DOCKER_BIN logout
                    '''
                }
            }
        }
    }
}