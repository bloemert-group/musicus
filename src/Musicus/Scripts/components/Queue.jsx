import React, { PropTypes } from 'react'
import { connect } from 'react-redux';
import QueueItem from './QueueItem.jsx';

class Queue extends React.Component {
  render() {
		var queuelist = [];
    for (var i = 0; i < this.props.queue.length; i++) {
      queuelist.push({
        Description: this.props.queue[i].artist + ' - ' + this.props.queue[i].description,
        TrackLength: this.props.queue[i].trackLength,
        QueueId: i
      });
    }

    return (
        <div className="body-queue-panel">
          <div className="body-queue-panel-header">
            <i className="list icon"></i> Afspeellijst
          </div>
          <div className="body-queue-panel-content">
            {
                queuelist.map(line =>
                  <QueueItem description={line.Description} tracklength={line.TrackLength} active={line.QueueId === 0}  />
                )
            }
          </div>
        </div>
    )
  }
}

Queue.propTypes = {
}

function mapStateToProps(state) {
  return {
		queue: state.musicusState.queue
  }
}

export default connect(mapStateToProps)(Queue)
