import time

def get_stack(input_string):
    # Se invierte la cadena para simular la pila
    return list(reversed(input_string))

def error():
    print("\nError: La cadena no es válida")

def valido(token):
    print("\n" + token + " » " + str(get_token_number(token)))
    return ""

def get_token_number(text):
    first_character_from_text = text[0]
    last_character_from_text = text[-1]

    character_token_first = {
# Define aquí los tokens correspondientes
}

character_token_last = {
# Define aquí los tokens correspondientes
}

reservadas_values = {
# Define aquí los valores reservados
}

tokens_con_referencia = [
        # Lista de tokens con referencia
    ]

    for key, value in character_token_last.items():
        if last_character_from_text in key and first_character_from_text in character_token_first.get(value, []):
            if value in tokens_con_referencia:
if text.upper() in reservadas_values:
return reservadas_values[text.upper()]
            return value
    return 0

def analizar_texto(input_stack):
    estado = 0
    actual_text = ""

    while input_stack:
        actual_text = actual_text.strip()
        actual_char = input_stack.pop()

        if actual_char != ' ' and actual_char != chr(0):
            if estado == 0:
                if ord('0') <= ord(actual_char) <= ord('9'):
                    estado = 1
                elif actual_char == '=':
                    estado = 2
                elif actual_char == ':':
                    estado = 3
                elif(ord('A') <= ord(actual_char) <= ord('Z')) or(ord('a') <= ord(actual_char) <= ord('z')) or actual_char == '_':
                    estado = 4
                else:
                    error()
                    return
            elif estado == 1:
                if not(ord('0') <= ord(actual_char) <= ord('9')):
                    if actual_text:
                        actual_text = valido(actual_text)
                    estado = 0
                    input_stack.append(actual_char)
                    actual_char = ' '
            elif estado == 2:
                if actual_text:
                    actual_text = valido(actual_text)
                estado = 0
                input_stack.append(actual_char)
                actual_char = ' '
            elif estado == 3:
                if actual_char == '=':
                    estado = 2
                else:
                    error()
                    return
            elif estado == 4:
                if not((ord('0') <= ord(actual_char) <= ord('9')) or(ord('A') <= ord(actual_char) <= ord('Z')) or(ord('a') <= ord(actual_char) <= ord('z')) or actual_char == '_'):
                    if actual_text:
                        actual_text = valido(actual_text)
                    estado = 0
                    input_stack.append(actual_char)
                    actual_char = ' '
            actual_text += actual_char

        else:
            if actual_text and(estado == 1 or estado == 2 or estado == 4):
                actual_text = valido(actual_text)
            else:
                error()
            return

def main():
    while True:
        print("\nEscriba la cadena para analizar: ", end = "")
        input_text = input()
        input_stack = get_stack(input_text)

        print("\nAnalizando", end = "")
        time.sleep(0.3)
        print(".", end = "")
        time.sleep(0.3)
        print(".", end = "")
        time.sleep(0.8)
        print(".")

        try:
            analizar_texto(input_stack)
        except Exception as e:
            print("\nError: " + str(e))

        print("\nPresiona cualquier tecla para continuar.")
        input()
        print()

if __name__ == "__main__":
    main()
