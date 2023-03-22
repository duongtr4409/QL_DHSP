import { all, takeEvery, put, call } from "redux-saga/effects";
import api from "../../containers/ThamSoHeThong/config";
import actions from "./actions";

function* getInitData({ payload }) {
  try {
    const response = yield call(api.DanhSachThamSo, payload.filterData);
    const getPageSize = yield call(api.GetByKey, { ConfigKey: "PAGE_SIZE" });
    yield put({
      type: actions.THAMSO_GET_INIT_DATA_REQUEST_SUCCESS,
      payload: {
        DanhSachThamSo: response.data.Data,
        TotalRow: response.data.TotalRow,
        defaultPageSize: getPageSize.data.Data.ConfigValue,
      },
    });
  } catch (e) {
    yield put({
      type: actions.THAMSO_GET_INIT_DATA_REQUEST_ERROR,
    });
  }
}

function* getList({ payload }) {
  try {
    const response = yield call(api.DanhSachThamSo, payload.filterData);
    yield put({
      type: actions.THAMSO_GET_LIST_REQUEST_SUCCESS,
      payload: {
        DanhSachThamSo: response.data.Data,
        TotalRow: response.data.TotalRow,
      },
    });
  } catch (e) {
    yield put({
      type: actions.THAMSO_GET_LIST_REQUEST_ERROR,
    });
  }
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.THAMSO_GET_INIT_DATA_REQUEST, getInitData)]);

  yield all([yield takeEvery(actions.THAMSO_GET_LIST_REQUEST, getList)]);
}
