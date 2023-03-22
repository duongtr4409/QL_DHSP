import { all, takeEvery, put, call } from "redux-saga/effects";
import api from "../../containers/CheckinOut/config";
import actions from "./actions";

function* getInitData() {
  try {
    const responseCanBo = yield call(api.DanhSachCanBo);
    const responseCameraIP = yield call(api.GetIPCamera, {ConfigKey: "Camera_Path"});
    //return action
    yield put({
      type: actions.CHECKIN_GET_DATA_REQUEST_SUCCESS,
      payload: {
        DanhSachCanBo: responseCanBo.data.Data,
        IPCamera: responseCameraIP.data.Data.ConfigValue
      }
    });
  } catch (e) {
    yield put({
      type: actions.CHECKIN_GET_DATA_REQUEST_ERROR
    });
  }
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.CHECKIN_GET_DATA_REQUEST, getInitData)]);
}
