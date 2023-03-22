import { all, takeEvery, put, call } from "redux-saga/effects";
import api from "../../containers/ThongBao/config";
import apiCanBo from "../../containers/QLTaiKhoan/config";
import actions from "./actions";

function* getInitData({ payload }) {
  try {
    const response = yield call(api.DanhSachThongBao, payload.filterData);
    // const canbo = yield call(apiCanBo.DanhSachTaiKhoan, {PageSize: 999999999, PageNumber: 1});
    yield put({
      type: actions.THONGBAO_GET_INIT_DATA_REQUEST_SUCCESS,
      payload: {
        DanhSachThongBao: response.data.Data,
        // DanhSachCanBo: canbo.data.Data,
        TotalRow: response.data.TotalRow,
      },
    });
  } catch (e) {
    yield put({
      type: actions.THONGBAO_GET_INIT_DATA_REQUEST_ERROR,
    });
  }
}

function* getList({ payload }) {
  try {
    const response = yield call(api.DanhSachThongBao, payload.filterData);
    yield put({
      type: actions.THONGBAO_GET_LIST_REQUEST_SUCCESS,
      payload: {
        DanhSachThongBao: response.data.Data,
        TotalRow: response.data.TotalRow,
      },
    });
  } catch (e) {
    yield put({
      type: actions.THONGBAO_GET_LIST_REQUEST_ERROR,
    });
  }
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.THONGBAO_GET_INIT_DATA_REQUEST, getInitData)]);
  yield all([yield takeEvery(actions.THONGBAO_GET_LIST_REQUEST, getList)]);
}
