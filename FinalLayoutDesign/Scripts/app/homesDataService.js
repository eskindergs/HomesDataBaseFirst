﻿var httpVerbs = {
    POST: 'POST',
    PUT: 'PUT',
    GET: 'GET',
    DEL: 'DELETE'
};

var homesDataService = (function () {

    var
    ds = {
        //send request to the server
        commit: function (type, url, data) {

            // Remove 'id' member to perpare for INSERT
            if (type === httpVerbs.POST) {
                delete data['id'];
            }

            return $.ajax({
                type: type,
                url: url,
                data: data,
                dataType: 'json'
            });
        },

        del: function (data) {
            return this.commit(httpVerbs.DEL, '/api/homes/' + data.id);
        },

        save: function (data) {

            var
                type = httpVerbs.POST,
                url = '/api/homes';

            if (data.id > 0) {
                type = httpVerbs.PUT;
                url += '/' + data.id;
            }

            return this.commit(type, url, data);
        },



    };//ds
    _.bindAll(ds, 'del', 'save');
    return {
        save: ds.save,
        del: ds.del
    }


})();//homeservice