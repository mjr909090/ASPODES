var commonFilter = angular.module("commonFilter", []);
commonFilter.filter('changeStatus', function () {
    return function (status) {
        var statusName;

        switch (status) {
            case 0:
                statusName = 新建;
                break;
            case 1:
                statusName = 新建;
                break;
            case 2:
                statusName = 新建;
                break;
            case 3:
                statusName = 新建;
                break;
            case 4:
                statusName = 新建;
                break;
            case 5:
                statusName = 待单位审核;
                break;
            case 6:
                statusName = 被单位管理员驳回;
                break;
            case 7:
                statusName = 被单位管理员撤回;
                break;
            case 8:
                statusName = 待院管理员受理;
                break;
            case 9:
                statusName = 院不受理;
                break;
            case 10:
                statusName = 等待指派专家;
                break;
            case 11:
                statusName = 等待手动指派;
                break;
            case 12:
                statusName = 等待发送指派信息;
                break;
            case 13:
                statusName = 评审中;
                break;
            case 14:
                statusName = 评审完毕;
                break;
            case 15:
                statusName = 不资助;
                break;
            case 16:
                statusName = 在库;
                break;
            case 17:
                statusName = 已出库;
                break;
            case 18:
                statusName = 过期失效;
                break;
        }


        return statusName;
    }
})