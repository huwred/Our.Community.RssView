angular.module("umbraco").controller("RssViewController", function ($scope, $http) {
    var vm = this;
    vm.status = {
        linkTested:false,
        linkValid: false,
        isChecking: false,
        buttonText: 'Check Feed Url'
    }

    vm.overlay = {
        show:false,
        feed: {},
        view: '/app_plugins/RssView/preview.html',
        close: function (oldModel) {
            vm.overlay.show = false;
        },
        title: "Feed Preview"
    }

    vm.checkFeedUrl = checkFeedUrl;
    vm.updateValue = updateValue;
    vm.previewFeed = previewFeed;

    function previewFeed() {
        vm.overlay.title = "Feed Preview";
        vm.overlay.show = true;
    }
    function updateValue() {
        resetCheck();
    }
    function resetCheck() {
        vm.status.linkTested = false;
        vm.status.linkValid = false;
        vm.status.isChecking = false;
        vm.status.statusMessage = '';
        vm.status.buttonText = 'Check Feed Url';
    }
    function checkFeedUrl() {
        if (!vm.status.linkTested) {
            resetCheck();
            vm.status.isChecking = true;
            $http({
                method: 'GET',
                url:  '/umbraco/api/feed/GetRssFeed?feedUrl=' + $scope.model.value
            }).then(function successCallback(response) {
                
                var feedDetails = response.data;
                vm.status.isChecking = false;
                vm.status.linkTested = true;

                if (response.data.hasFeedResults) {

                    vm.overlay.feed = feedDetails;
                    vm.overlay.title = feedDetails.syndicationFeed.title.Text;
                    vm.status.linkValid = true;
                    vm.status.statusMessage = '';
                    vm.status.buttonText = 'Feed Url is Valid';
                }
                else {
                    vm.status.linkValid = false;
                    vm.status.statusMessage = 'A valid Rss Feed was not found at this Url';
                    vm.status.buttonText = 'Not Valid';
                }

            }, function errorCallback(response) {
                vm.status.linkTested = true;
                vm.status.linkValid = false;
                vm.status.isChecking = false;
                vm.status.statusMessage = 'An error requesting and reading the feed url';

            });
        }
    }
});