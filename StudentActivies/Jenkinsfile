pipeline{
    // chỉ định pipeline có thể chạy trên bất kỳ agent nào
    agent any

    stages {
        // giai đoạn 1: kéo code mới nhất từ git
        stage('checkout') {
            steps {
                git url: 'https://github.com/dieuanh2k4/Student-Activities-Management.git', branch: 'main'
            }
        }
        
        // giai đoạn 2: khôi phục các gói NuGet
        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        // giai đoạn 3: build dự án
        stage('Build') {
            steps {
                bat 'dotnet build --configuration Release --no-restore'
            }
        } 

        // giai đoạn 4: Chạy unit test
        stage('Test') {
            steps {
                bat 'dotnet test --no-build --configuration Release'
            }
        }

        // giai đoạn 5: đóng gói ứng dụng
        stage('Publish') {
            steps {
                bat 'dotnet publish --configuration Release --output ./publish --no-build'
            }
        }

        // giai đoạn 6: lưu trữ 'Artifacts'
        stage('Archive') {
            steps {
                archiveArtifacts artifacts: '**/publish/**'
            }
        }
    }

    post {
        always {
            echo 'Pipeline đã hoàn thành'
            cleanWs()
        }

        success {
            echo 'Build and Test thành công'
        }

        failure {
            echo 'Build hoặc Test thất bại'
        }
    }
}