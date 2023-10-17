from flask import Flask, render_template, request
import subprocess

app = Flask(__name__)


@app.route("/", methods=["GET", "POST"])
def index():
    output = ""
    if request.method == "POST":
        command = request.form["command"]
        try:
            output = subprocess.check_output(
                command, shell=True, stderr=subprocess.STDOUT
            ).decode("utf-8")
        except subprocess.CalledProcessError as e:
            output = str(e)

    return render_template("index.html", output=output)


if __name__ == "__main__":
    app.run(debug=True)
