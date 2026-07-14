USER := $(shell whoami)

up:
	docker compose up --build

down:
	docker compose down -v

fix-host-permissions:
	sudo chown -R $(USER):$(USER) ./api/bin ./api/obj
	cd ./api && dotnet restore

migrate:
	docker exec -it -u $$(id -u):$$(id -g) api dotnet ef database update

seed:
	curl -X POST http://localhost:8080/dev/seed

logs:
	docker compose logs -f api

restart:
	docker compose restart api

test:
	docker exec -it api teapie test Tests

test-collection:
	docker exec -it api teapie test Tests/$(collection)

test-single:
	docker exec -it api teapie test $(file)