import React from "react";
import {all, takeEvery, put, fork, call} from "redux-saga/effects";
import api from "../../containers/Topbar/config";
import apithongbao from "../../customApp/containers/ThongBao/config";
import apithamso from "../../customApp/containers/ThamSoHeThong/config";
import DashboardNotify from "../../customApp/containers/ThongBao/dashboardNotify";
import {message, notification, Icon, Modal, Tooltip} from "antd";

const notifyRef = React.createRef();
let listKey = {};

export function* removeNotification() {
  notification.destroy();
}

export function* getNotification() {
  const response = yield call(api.getNotifications);
  // const responseHuongDan = yield call(api.danhSachHuongDan);
  if (response.data.Status && response.data.Status > 0 && response.data.Data.length !== 0) {
    notification.destroy();
    notification.warning({
      // getContainer: (node) => {
      //   console.log(node);
      // },
      style: {
        // marginLeft: 0,
        width: window.innerWidth > 640 ? 640 : window.innerWidth,
        marginLeft: window.innerWidth > 600 ? 335 - 600 : 0,
      },
      icon: <i/>,
      message: "",
      description: (
        <DashboardNotify
          closeNotify={() => {
            notification.destroy();
          }}
          data={response.data.Data}
        />
      ),
      placement: "bottomRight",
      duration: 0,
    });
  }
}

export function* getConfig() {
  const response = yield call(apithamso.DanhSachThamSo, {PageSize: 90000});
  if (response.data.Status && response.data.Status > 0) {
    yield put({
      type: "GET_CONFIG_SUCCESS",
      data: response.data.Data,

      // DanhSachHuongDan: responseHuongDan.data.Data
    });
  } else {
    message.warn("Lấy danh sách tham số hệ thống thất bại");
  }
}

export default function* rootSaga() {
  yield all([takeEvery("LOGIN_SUCCESS", getConfig), takeEvery("LOGIN_SUCCESS", getNotification), takeEvery("LOGOUT", removeNotification)]);
}
