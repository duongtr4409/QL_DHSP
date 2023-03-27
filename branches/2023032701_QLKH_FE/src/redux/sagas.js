import { all } from 'redux-saga/effects';
import devSagas from '../customApp/redux/sagas';
import authSagas from './auth/saga';
import appSagas from './app/saga';

export default function* rootSaga(getState) {
  yield all([
    devSagas(),
    authSagas(),
    appSagas(),
  ]);
}
