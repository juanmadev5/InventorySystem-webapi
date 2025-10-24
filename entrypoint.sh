#!/bin/bash
set -e

DB_HOST="db"
DB_PORT="3306"
TIMEOUT=45

echo "Esperando hasta $TIMEOUT segundos a que la base de datos MySQL en $DB_HOST:$DB_PORT esté disponible..."

start_time=$(date +%s)
while ! nc -z $DB_HOST $DB_PORT; do
  current_time=$(date +%s)
  elapsed_time=$((current_time - start_time))
  if [ $elapsed_time -ge $TIMEOUT ]; then
    echo "Error: La base de datos no está disponible después de $TIMEOUT segundos. Saliendo."
    exit 1
  fi
  echo "MySQL aún no está lista ($elapsed_time segundos transcurridos). Reintentando en 1 segundo..."
  sleep 1
done

echo "Base de datos disponible. Ejecutando migraciones..."
dotnet InventorySystem-webapi.dll --migrate

MIGRATION_EXIT_CODE=$?
if [ $MIGRATION_EXIT_CODE -ne 0 ]; then
  echo "ERROR: Fallo al aplicar las migraciones. Código de salida: $MIGRATION_EXIT_CODE"
  exit $MIGRATION_EXIT_CODE
fi

echo "Migraciones aplicadas con éxito. Iniciando la API..."
exec dotnet InventorySystem-webapi.dll