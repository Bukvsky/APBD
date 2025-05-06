#!/bin/bash



BASE_URL="http://localhost:5128/api"

GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
RED='\033[0;31m'
NC='\033[0m' 


function print_test_info() {
  echo -e "\n${CYAN}=======================================${NC}"
  echo -e "${CYAN}TEST: $1${NC}"
  echo -e "${CYAN}=======================================${NC}"
}

# -------------------------------------------------------
# 1. Pobranie wszystkich wycieczek
# -------------------------------------------------------
print_test_info "Pobranie wszystkich wycieczek (GET /api/trips)"

TRIPS_RESPONSE=$(curl -s "$BASE_URL/trips")
echo -e "${GREEN}$TRIPS_RESPONSE${NC}"


FIRST_TRIP_ID=$(echo $TRIPS_RESPONSE | grep -o '"id":[0-9]*' | head -1 | cut -d':' -f2)
if [ -z "$FIRST_TRIP_ID" ]; then
  echo -e "${RED}Nie znaleziono żadnych wycieczek!${NC}"
  FIRST_TRIP_ID=1  # Domyślna wartość
else
  echo -e "${YELLOW}Pierwsze ID wycieczki: $FIRST_TRIP_ID${NC}"
fi

# -------------------------------------------------------
# 2. Pobranie szczegółów jednej wycieczki
# -------------------------------------------------------
print_test_info "Pobranie szczegółów wycieczki (GET /api/trips/{id})"

TRIP_RESPONSE=$(curl -s "$BASE_URL/trips/$FIRST_TRIP_ID")
echo -e "${GREEN}$TRIP_RESPONSE${NC}"

# -------------------------------------------------------
# 3. Utworzenie nowego klienta
# -------------------------------------------------------
print_test_info "Utworzenie nowego klienta (POST /api/clients)"

NEW_CLIENT='{
  "firstName": "Jan",
  "lastName": "Kowalski",
  "email": "jan.kowalski@example.com",
  "telephone": "123456789",
  "pesel": "90010112345"
}'

CREATE_CLIENT_RESPONSE=$(curl -s -X POST "$BASE_URL/clients" \
  -H "Content-Type: application/json" \
  -d "$NEW_CLIENT")
echo -e "${GREEN}$CREATE_CLIENT_RESPONSE${NC}"


CLIENT_ID=$(echo $CREATE_CLIENT_RESPONSE | grep -o '"id":[0-9]*' | cut -d':' -f2)
echo -e "${YELLOW}ID nowego klienta: $CLIENT_ID${NC}"

# -------------------------------------------------------
# 4. Pobranie wycieczek klienta (początkowo pustych)
# -------------------------------------------------------
print_test_info "Pobranie wycieczek klienta (GET /api/clients/{id}/trips)"

CLIENT_TRIPS_RESPONSE=$(curl -s "$BASE_URL/clients/$CLIENT_ID/trips")
echo -e "${GREEN}$CLIENT_TRIPS_RESPONSE${NC}"

# -------------------------------------------------------
# 5. Rejestracja klienta na wycieczkę
# -------------------------------------------------------
print_test_info "Rejestracja klienta na wycieczkę (PUT /api/clients/{id}/trips/{tripId})"

REGISTER_CLIENT_RESPONSE=$(curl -s -X PUT "$BASE_URL/clients/$CLIENT_ID/trips/$FIRST_TRIP_ID")
echo -e "${GREEN}$REGISTER_CLIENT_RESPONSE${NC}"

# -------------------------------------------------------
# 6. Ponowne pobranie wycieczek klienta (powinny zawierać dodaną wycieczkę)
# -------------------------------------------------------
print_test_info "Pobranie wycieczek klienta po rejestracji"

CLIENT_TRIPS_AFTER_REG=$(curl -s "$BASE_URL/clients/$CLIENT_ID/trips")
echo -e "${GREEN}$CLIENT_TRIPS_AFTER_REG${NC}"

# -------------------------------------------------------
# 7. Usunięcie rejestracji klienta z wycieczki
# -------------------------------------------------------
print_test_info "Usunięcie rejestracji klienta z wycieczki (DELETE /api/clients/{id}/trips/{tripId})"

UNREGISTER_CLIENT_RESPONSE=$(curl -s -X DELETE "$BASE_URL/clients/$CLIENT_ID/trips/$FIRST_TRIP_ID")
echo -e "${GREEN}$UNREGISTER_CLIENT_RESPONSE${NC}"

# -------------------------------------------------------
# 8. Ponowne pobranie wycieczek klienta (powinno być puste)
# -------------------------------------------------------
print_test_info "Pobranie wycieczek klienta po usunięciu rejestracji"

CLIENT_TRIPS_AFTER_UNREG=$(curl -s "$BASE_URL/clients/$CLIENT_ID/trips")
echo -e "${GREEN}$CLIENT_TRIPS_AFTER_UNREG${NC}"


echo -e "\n${YELLOW}=======================================${NC}"
echo -e "${YELLOW}TESTY ZAKOŃCZONE${NC}"
echo -e "${YELLOW}=======================================${NC}"
echo -e "Sprawdź wyniki powyżej, aby upewnić się, że wszystkie operacje zakończyły się sukcesem."
