
docker image build -t sixeyed/petition-watcher-index-handler -f docker\index-handler\Dockerfile .

docker image build -t sixeyed/petition-watcher-scheduler-api -f docker\scheduler-api\Dockerfile .