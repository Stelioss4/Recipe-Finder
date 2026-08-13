pipeline {
    agent any

    options {
        skipDefaultCheckout(true)
        disableConcurrentBuilds()
    }

    environment {
        DOCKER_IMAGE = 'steliosboursanidis/recipefinderwebapp'
        DOCKER_BIN = '/snap/docker/current/bin/docker'
        ENV_FILE = '/etc/recipefinder/recipefinder.env'
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

        stage('Deploy Production') {
            steps {
                sh '''
                    set -e

                    rm -f previous-image.txt deployment-started.txt

                    PREVIOUS_IMAGE=$($DOCKER_BIN inspect \
                        --type=container \
                        --format='{{.Image}}' \
                        recipefinder 2>/dev/null || true)

                    echo "$PREVIOUS_IMAGE" > previous-image.txt

                    echo "Previous production image: $PREVIOUS_IMAGE"
                    echo "Deploying: $DOCKER_IMAGE:${BUILD_NUMBER}"

                    if $DOCKER_BIN inspect recipefinder >/dev/null 2>&1; then
                        $DOCKER_BIN stop recipefinder
                        $DOCKER_BIN rm recipefinder
                    fi

                    touch deployment-started.txt

                    $DOCKER_BIN run -d \
                        --name recipefinder \
                        --restart unless-stopped \
                        -p 8080:8080 \
                        -p 8443:443 \
                        --env-file "$ENV_FILE" \
                        $DOCKER_IMAGE:${BUILD_NUMBER}
                '''
            }

            post {
                failure {
                    sh '''
                        if [ -f deployment-started.txt ]; then
                            echo "Deploy failed after production replacement started."
                            echo "Rolling back..."

                            PREVIOUS_IMAGE=$(cat previous-image.txt)

                            $DOCKER_BIN stop recipefinder 2>/dev/null || true
                            $DOCKER_BIN rm recipefinder 2>/dev/null || true

                            if [ -n "$PREVIOUS_IMAGE" ]; then
                                $DOCKER_BIN run -d \
                                    --name recipefinder \
                                    --restart unless-stopped \
                                    -p 8080:8080 \
                                    -p 8443:443 \
                                    --env-file "$ENV_FILE" \
                                    "$PREVIOUS_IMAGE"

                                echo "Rollback completed."
                            else
                                echo "No previous image was available for rollback."
                            fi
                        fi
                    '''
                }
            }
        }

        stage('Health Check') {
            steps {
                sh '''
                    echo "Waiting for Recipe Finder to start..."
                    sleep 10

                    for i in 1 2 3 4 5 6; do
                        if curl -fsS --max-time 10 \
                            http://127.0.0.1:8080/ \
                            > /dev/null; then

                            echo "Recipe Finder is healthy."
                            exit 0
                        fi

                        echo "Health check attempt $i failed."
                        sleep 5
                    done

                    echo "Recipe Finder health check failed."
                    exit 1
                '''
            }

            post {
                failure {
                    sh '''
                        echo "New production version is unhealthy."
                        echo "Rolling back..."

                        PREVIOUS_IMAGE=$(cat previous-image.txt)

                        $DOCKER_BIN stop recipefinder 2>/dev/null || true
                        $DOCKER_BIN rm recipefinder 2>/dev/null || true

                        if [ -n "$PREVIOUS_IMAGE" ]; then
                            $DOCKER_BIN run -d \
                                --name recipefinder \
                                --restart unless-stopped \
                                -p 8080:8080 \
                                -p 8443:443 \
                                --env-file "$ENV_FILE" \
                                "$PREVIOUS_IMAGE"

                            echo "Rollback completed."

                            sleep 10

                            if curl -fsS --max-time 10 \
                                http://127.0.0.1:8080/ \
                                > /dev/null; then
                                echo "Previous production version is healthy again."
                            else
                                echo "WARNING: rollback container started, but health check still failed."
                            fi
                        else
                            echo "No previous image was available for rollback."
                        fi
                    '''
                }
            }
        }
    }

    post {
        success {
            echo "CI/CD completed successfully. Recipe Finder is live."
        }

        failure {
            echo "CI/CD failed. Check the failed stage and rollback output."
        }
    }
}