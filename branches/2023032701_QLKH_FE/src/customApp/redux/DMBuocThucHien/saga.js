import { all, takeEvery, put, call } from "redux-saga/effects";
import api from "../../containers/DMBuocThucHien/config";
import actions from "./actions";

function* getInitData({payload}) {
  try {
    const response = yield call(api.DanhSachBuocThucHien, payload.filterData);
    yield put({
      type: actions.BUOCTHUCHIEN_GET_INIT_DATA_REQUEST_SUCCESS,
      payload: {
        DanhSachBuocThucHien: response.data.Data,
        TotalRow: response.data.TotalRow,
      }
    });
  } catch (e) {
    yield put({
      type: actions.BUOCTHUCHIEN_GET_INIT_DATA_REQUEST_ERROR
    });
  }
}

function* getList({payload}) {
  try {
    const response = yield call(api.DanhSachBuocThucHien, payload.filterData);
    yield put({
      type: actions.BUOCTHUCHIEN_GET_LIST_REQUEST_SUCCESS,
      payload: {
        DanhSachBuocThucHien: response.data.Data,
        TotalRow: response.data.TotalRow,
      }
    });
  } catch (e) {
    yield put({
      type: actions.BUOCTHUCHIEN_GET_LIST_REQUEST_ERROR
    });
  }
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.BUOCTHUCHIEN_GET_INIT_DATA_REQUEST, getInitData)]);
  yield all([yield takeEvery(actions.BUOCTHUCHIEN_GET_LIST_REQUEST, getList)]);
}
