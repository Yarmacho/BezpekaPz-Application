document.querySelector("input[type='file']").addEventListener('change', function (e) {
    if (e.target.value) {
        e.target.form.submit();
    }
});