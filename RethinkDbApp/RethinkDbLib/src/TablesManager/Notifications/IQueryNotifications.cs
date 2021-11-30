using System;
using System.Collections.Generic;

namespace Rethink.Model
{
    /// <summary>
    /// Gestione tabella Notifications ----> select, update, delete
    /// </summary>
    public interface IQueryNotifications
    {
        /// <summary>
        /// Inserisce sul db "Notification" una nuova notifica
        /// </summary>
        /// <typeparam name="T">Deve essere una classe che eredita dalla classe astratta "Notification"</typeparam>
        /// <param name="notification">Notifica che verrà inserita sul db</param>
        public void NewNotification<T>(T notification) where T : Notification;

        /// <summary>
        /// Elimina la notifica con l'id passato in input
        /// </summary>
        /// <param name="id">id notifica</param>
        public void DeleteNotification(Guid id);

        /// <summary>
        /// Richiede al db la notifica con l'id passato in input
        /// </summary>
        /// <typeparam name="T">Deve essere una classe che eredita dalla classe astratta "Notification"</typeparam>
        /// <param name="id">id della notifica da ricercare</param>
        /// <returns>La notifica</returns>
        public T GetNotificationOrNull<T>(Guid id) where T : Notification;

        /// <summary>
        /// Richiede al db tutte le notifiche avvenute in una certa data, è sufficente impostare giorno/mese/anno
        /// </summary>
        /// <typeparam name="T">Deve essere una classe che eredita dalla classe astratta "Notification"</typeparam>
        /// <param name="date">data presa in considerazione, è sufficente impostargli giorno/mese/anno</param>
        /// <returns>Lista delle notifiche in quella data</returns>
        public IList<T> GetNotifications<T>(DateTime date) where T : Notification;

        //public IList<String> GetNotifications(DateTime date);

        /// <summary>
        /// Richiede al db tutte le notifiche con un certo testo
        /// </summary>
        /// <typeparam name="T">Deve essere una classe che eredita dalla classe astratta "Notification"</typeparam>
        /// <param name="text">Testo preso in considerazione</param>
        /// <returns>Lista di notifiche con quel testo</returns>
        public IList<T> GetNotificationsWithText<T>(String text) where T : Notification;

        /// <summary>
        /// Richiede al db tutte le notifiche con un certo argomento
        /// </summary>
        /// <typeparam name="T">Deve essere una classe che eredita dalla classe astratta "Notification"</typeparam>
        /// <param name="arg">Argomento richiesto</param>
        /// <returns>Lista di notifiche con quel arg</returns>
        public IList<T> GetNotificationsWithArg<T>(String arg) where T : Notification;
    }
}
