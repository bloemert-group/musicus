import { combineReducers } from 'redux'
import { musicusReducer } from './musicus.jsx'

const rootReducer = combineReducers({
  musicusState: musicusReducer 
});

export default rootReducer;