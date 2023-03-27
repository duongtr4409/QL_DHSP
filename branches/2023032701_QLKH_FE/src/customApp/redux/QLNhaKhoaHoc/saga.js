import {all, takeEvery, put, call} from "redux-saga/effects";
import api from "../../containers/QLNhaKhoaHoc/config";
import apiConfig from "../../containers/ThamSoHeThong/config"
import apiCore from "../../containers/DataCoreAPI/config"
import actions from "./actions";

function* getInitData({payload}) {
  try {
    const response = yield call(api.DanhSachNhaKhoaHoc, payload.filterData);
    const configRes = yield call(apiConfig.GetByKey, {ConfigKey: 'FILE_LIMIT'});
    const chucdanh = yield call(apiCore.DanhSachChucVu);
    const hocvi = yield call(apiCore.DanhSachHocVi);
    const fileallow = yield call(apiConfig.GetByKey, {ConfigKey: 'LOAI_FILE'});
    yield put({
      type: actions.QLNHAKHOAHOC_GET_INIT_DATA_REQUEST_SUCCESS,
      payload: {
        DanhSachNhaKhoaHoc: response.data.Data,
        TotalRow: response.data.TotalRow,
        FileLimit: configRes.data.Data.ConfigValue,
        DanhSachChucDanh: chucdanh.data.Data,
        DanhSachHocVi: hocvi.data.Data,
        FileAllow: fileallow.data.Data.ConfigValue
      }
    });
  } catch (e) {
    yield put({
      type: actions.QLNHAKHOAHOC_GET_INIT_DATA_REQUEST_ERROR
    });
  }
}

function* getList({payload}) {
  try {
    const response = yield call(api.DanhSachNhaKhoaHoc, payload.filterData);
    yield put({
      type: actions.QLNHAKHOAHOC_GET_LIST_REQUEST_SUCCESS,
      payload: {
        DanhSachNhaKhoaHoc: response.data.Data,
        TotalRow: response.data.TotalRow,
      }
    });
  } catch (e) {
    yield put({
      type: actions.QLNHAKHOAHOC_GET_LIST_REQUEST_ERROR
    });
  }
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.QLNHAKHOAHOC_GET_INIT_DATA_REQUEST, getInitData)]);
  yield all([yield takeEvery(actions.QLNHAKHOAHOC_GET_LIST_REQUEST, getList)]);
}
