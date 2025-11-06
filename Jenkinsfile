pipeline {
    agent any

    environment {
        BACKEND_DIR = "Portal-Agro-comercial-del-Huila"
        FRONTEND_DIR = "frontend"
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'trespa', url: 'https://github.com/Leguizamo2502/Portal-Agro-comercial-del-Huila.git'
            }
        }

        stage('Build Backend (.NET)') {
            steps {
                dir("${BACKEND_DIR}") {
                    sh 'dotnet restore'
                    sh 'dotnet build --configuration Release'
                }
            }
        }

        stage('Build Frontend (Angular)') {
            steps {
                dir("${FRONTEND_DIR}") {
                    sh 'npm install'
                    sh 'ng build --configuration production'
                }
            }
        }

        stage('Publish Backend') {
            steps {
                dir("${BACKEND_DIR}") {
                    sh 'dotnet publish -c Release -o out'
                }
            }
        }

        stage('Deploy') {
            steps {
                echo 'Desplegando la aplicación...'
                // Aquí puedes añadir comandos para Docker o subir los archivos a un servidor
            }
        }
    }

    post {
        success {
            echo 'Pipeline completado con éxito'
        }
        failure {
            echo 'Error en el pipeline'
        }
    }
}
