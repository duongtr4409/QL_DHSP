import {all, takeEvery, put, call} from "redux-saga/effects";
import api from "../../containers/QLHoiDong/config";
import apiCore from "../../containers/DataCoreAPI/config"
import actions from "./actions";
import {formatDataTreeSelect} from '../../../helpers/utility';

function* getInitData({payload}) {
  try {
    const response = yield call(api.DanhSachHoiDong, payload.filterData);
    const canbo = yield call(apiCore.DanhSachCanBo, {departmentid: 0});
    const capquanly = yield call(api.DanhSachCapQuanLy);
    yield put({
      type: actions.QLHOIDONG_GET_INIT_DATA_REQUEST_SUCCESS,
      payload: {
        DanhSachHoiDong: response.data.Data,
        DanhSachCanBo: canbo.data.Data,
        TotalRow: response.data.TotalRow,
        DanhSachCapQuanLy: formatDataTreeSelect(capquanly.data.Data)
      },
    });
  } catch (e) {
    yield put({
      type: actions.QLHOIDONG_GET_INIT_DATA_REQUEST_ERROR,
    });
  }
}

function* getList({payload}) {
  try {
    const response = yield call(api.DanhSachHoiDong, payload.filterData);
    yield put({
      type: actions.QLHOIDONG_GET_LIST_REQUEST_SUCCESS,
      payload: {
        DanhSachHoiDong: response.data.Data,
        TotalRow: response.data.TotalRow,
      },
    });
  } catch (e) {
    yield put({
      type: actions.QLHOIDONG_GET_LIST_REQUEST_ERROR,
    });
  }
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.QLHOIDONG_GET_INIT_DATA_REQUEST, getInitData)]);
  yield all([yield takeEvery(actions.QLHOIDONG_GET_LIST_REQUEST, getList)]);
}
