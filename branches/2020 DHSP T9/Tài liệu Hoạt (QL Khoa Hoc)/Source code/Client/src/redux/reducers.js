import DevReducers from '../customApp/redux/reducers';
import Auth from './auth/reducer';
import App from './app/reducer';
import ThemeSwitcher from './themeSwitcher/reducer';
import LanguageSwitcher from './languageSwitcher/reducer';

export default {
  ...DevReducers,
  Auth,
  App,
  ThemeSwitcher,
  LanguageSwitcher,
};
