angular.module('multiPagination', [])
    .directive('multiPagination', function () {
        return {
            restrict: "E",
            repalce: true,
            scope: {
                name: "@",
                pagination: "=",
                loadPage: "&"
            },
            template: '<div class="col-lg-8 col-md-9 col-sm-9 col-xs-12">' +
                            '<span style="font-size: 24px;" ng-cloak>共： {{pagination.pageItem}}页，{{pagination.count}} 条，当前第{{pagination.p_current}}页，显示&nbsp;{{pagination.p_count}}&nbsp;条</span>' +
                      '</div>' +
                      '<div class="pull-right">' +
                        '<ul class="pagination" style="margin: 0px;">' +
                            '<li ng-class="{true:\'disabled\'}[pagination.p_current  == 1]"><a ng-click="nextPage(pagination.p_current-1)">&laquo;</a></li>' +
                            ' <li ng-show="pagination.p_current  > 3"><a ng-click="nextPage(1)">1</a></li>' +
                            ' <li ng-show=" pagination.p_current  > 4">' +
                                '<a href="javascript:void(0)">...</a>' +
                            '</li>' +
                            '<li ng-repeat="page in pagination.pages" ng-if="judgeAbs(pagination.p_current, page)" ng-class="{true:\'active\'}[page == pagination.p_current ]">' +
                                ' <a ng-click="nextPage(page)">{{page}}</a>' +
                            '</li>' +
                            '<li ng-show=" pagination.p_current  < pagination.pageItem - 3">' +
                                ' <a href="javascript:void(0)">...</a>' +
                            '</li>' +
                            '<li ng-show="pagination.p_current  < pagination.pageItem - 2"><a ng-click="nextPage(pagination.pageItem)">{{pagination.pageItem}}</a></li>' +
                            '<li ng-class="{true:\'disabled\'}[pagination.p_current == pagination.pageItem || pagination.pageItem == 0]"><a ng-click="nextPage(pagination.p_current+1)">&raquo;</a></li>' +
                        '</ul>' +
                     '</div>',
            link: function (scope, elem, attrs) {
                //console.log(scope); //scope 为指令单独作用域
                scope.judgeAbs = function (current, page) {
                    if (Math.abs(current - page) < 3) {
                        return true;
                    } else
                        return false;
                }
                //选择某一页，判断是否发请求
                scope.nextPage = function (page) {
                    if (page <= scope.pagination.pageItem && page >= 1 && page != scope.pagination.p_current) {
                        scope.loadPage({ page: page, name: scope.name });
                    }
                }
            }
        }
    })