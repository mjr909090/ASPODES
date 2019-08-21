angular.module('selfPagination', [])
    .directive('selfPagination', function () {
        return {
            restrict: "E",
            repalce: true,
            template: '<div class="col-lg-8 col-md-9 col-sm-9 col-xs-12">' +
                            '<span style="font-size: 24px;" ng-cloak>共： {{pageItem}}页，{{count}} 条，当前第{{p_current}}页，显示&nbsp;{{p_count}}&nbsp;条</span>' +
                      '</div>' +
                      '<div class="pull-right">' +
                        '<ul class="pagination" style="margin: 0px;">' +
                            '<li ng-class="{true:\'disabled\'}[p_current  == 1]"><a ng-click="nextPage(p_current-1)">&laquo;</a></li>' +
                            ' <li ng-show="p_current  > 3"><a ng-click="nextPage(1)">1</a></li>' +
                            ' <li ng-show=" p_current  > 4">' +
                                '<a href="javascript:void(0)">...</a>' +
                            '</li>' +
                            '<li ng-repeat="page in pages" ng-if="judgeAbs(p_current, page)" ng-class="{true:\'active\'}[page == p_current ]">' +
                                ' <a ng-click="nextPage(page)">{{page}}</a>' +
                            '</li>' +
                            '<li ng-show=" p_current  < pageItem - 3">' +
                                ' <a href="javascript:void(0)">...</a>' +
                            '</li>' +
                            '<li ng-show="p_current  < pageItem - 2"><a ng-click="nextPage(pageItem)">{{pageItem}}</a></li>' +
                            '<li ng-class="{true:\'disabled\'}[p_current == pageItem || pageItem == 0]"><a ng-click="nextPage(p_current+1)">&raquo;</a></li>' +
                        '</ul>' +
                     '</div>',
            link: function (scope, elem, attrs) {                
                scope.judgeAbs = function (current, page) {
                    if (Math.abs(current - page) < 3) {
                        return true;
                    } else
                        return false;
                }
                //选择某一页，判断是否发请求
                scope.nextPage = function (page) {
                    if (page <= scope.pageItem && page >= 1 && page != scope.p_current) {
                        scope.load_page(page);
                    }
                }
            }
        }
    })