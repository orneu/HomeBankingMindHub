var app = new Vue({
    el: "#app",
    data: {
        accountInfo: {},
        //error: null
        errorToats: null,
        errorMsg: null,
    },
    methods: {
        getData: function () {
            const urlParams = new URLSearchParams(window.location.search);
            const id = urlParams.get('id');
            axios.get(`/api/accounts/${id}`)

                .then(function (response) {
                    //get client ifo
                    app.accountInfo = response.data;
                    app.accountInfo.transactions.sort((a, b) => parseInt(b.id - a.id))
                })
                .catch(function (error) {
                    if (error.response.status === 401) {
                        window.location.href = 'index.html'
                    }
                    // handle error
                    //app.error = error;
                    this.errorMsg = "Error getting data";
                    this.errorToats.show();
                })
        },
        formatDate: function (date) {
            return new Date(date).toLocaleDateString('en-gb');
        },

        signOut: function () {
            axios.post('/api/auth/logout')
                .then(response => window.location.href = "/index.html")
                .catch(() => {
                    this.errorMsg = "Sign out failed"
                    this.errorToats.show();
                })
        },
    },
    mounted: function () {
        this.errorToats = new bootstrap.Toast(document.getElementById('danger-toast'));
        this.getData();
    }
})