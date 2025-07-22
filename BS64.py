import base64

def image_to_base64(image_path):
    with open(image_path, "rb") as image_file:
        return base64.b64encode(image_file.read()).decode('utf-8')

# 画像をBase64に変換
base64_string = image_to_base64("IFoxer-Photoroom.png")
print(f"data:image/png;base64,{base64_string}")