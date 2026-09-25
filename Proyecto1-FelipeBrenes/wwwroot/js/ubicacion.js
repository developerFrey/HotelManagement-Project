document.addEventListener("DOMContentLoaded", function () {

    const data = {
        "San José": {
            "Escazú": ["San Rafael", "San Miguel"]
        },
        "Alajuela": {
            "Central": ["Alajuela"]
        }
    };

    document.querySelectorAll(".ubicacion-container").forEach(container => {

        const provincia = container.querySelector(".provincia");
        const canton = container.querySelector(".canton");
        const distrito = container.querySelector(".distrito");
        const hidden = container.querySelector(".ubicacionFinal");

        // Se cargan las provincias al cargar la página
        Object.keys(data).forEach(p => {
            provincia.innerHTML += `<option>${p}</option>`;
        });

        provincia.addEventListener("change", () => {
            canton.innerHTML = "<option value=''>Cantón</option>";
            distrito.innerHTML = "<option value=''>Distrito</option>";

            Object.keys(data[provincia.value] || {}).forEach(c => {
                canton.innerHTML += `<option>${c}</option>`;
            });

            actualizar();
        });

        canton.addEventListener("change", () => {
            distrito.innerHTML = "<option value=''>Distrito</option>";

            (data[provincia.value]?.[canton.value] || []).forEach(d => {
                distrito.innerHTML += `<option>${d}</option>`;
            });

            actualizar();
        });

        distrito.addEventListener("change", actualizar);

        function actualizar() {
            if (provincia.value && canton.value && distrito.value) {
                hidden.value = `${provincia.value} - ${canton.value} - ${distrito.value}`;
            }
        }

    });

});