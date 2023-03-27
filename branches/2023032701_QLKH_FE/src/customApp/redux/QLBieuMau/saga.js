 import { all, takeEvery, put, call } from "redux-saga/effects";
import api from "../../containers/QLBieuMau/config";
import apiConfig from "../../containers/ThamSoHeThong/config"
import apiBuocThucHien from "../../containers/DMBuocThucHien/config"
import actions from "./actions";

function* getInitData({payload}) {
  try {
    const getFileLimit = yield call(apiConfig.GetByKey, {ConfigKey: 'FILE_LIMIT'});
    const getFileAllow = yield call(apiConfig.GetByKey, {ConfigKey: 'LOAI_FILE'});
    const response = yield call(api.DanhSachBieuMau, payload.filterData);
    yield put({
      type: actions.BIEUMAU_GET_INIT_DATA_REQUEST_SUCCESS,
      payload: {
        DanhSachBieuMau: response.data.Data,
        TotalRow: response.data.TotalRow,
        FileLimit: getFileLimit.data.Data.ConfigValue,
        FileAllow: getFileAllow.data.Data.ConfigValue
      }
    });
  } catch (e) {
    yield put({
      type: actions.BIEUMAU_GET_INIT_DATA_REQUEST_ERROR
    });
  }
}

function* getList({payload}) {
  try {
    const response = yield call(api.DanhSachBieuMau, payload.filterData);
    yield put({
      type: actions.BIEUMAU_GET_LIST_REQUEST_SUCCESS,
      payload: {
        DanhSachBieuMau: response.data.Data,
        TotalRow: response.data.TotalRow
      }
    });
  } catch (e) {
    yield put({
      type: actions.BIEUMAU_GET_LIST_REQUEST_ERROR
    });
  }
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.BIEUMAU_GET_INIT_DATA_REQUEST, getInitData)]);
  yield all([yield takeEvery(actions.BIEUMAU_GET_LIST_REQUEST, getList)]);
}
